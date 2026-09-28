using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace BalancedDiet
{

    class BalancedDietHandbookPage : GuiHandbookPage
    {
        public string pageCode = "balanceddiet:guide-page";

        public string Title = "balanceddiet:guide-page-title";

        public string Text = "balanceddiet:guide-page-text";

        public string categoryCode = "guide";

        public LoadedTexture Texture;

        private RichTextComponentBase[] comps;

        private string titleCached;

        public override string PageCode => pageCode;

        public override string CategoryCode => categoryCode;

        public override float SearchWeightOffset => 1f;

        public override bool IsDuplicate => false;

        public override void Dispose()
        {
            Texture?.Dispose();
            Texture = null;
        }

        public void Init(ICoreClientAPI capi)
        {
            // Notice how we can inject custom text from the code side
            Text = Lang.Get(Text,
                BalancedDiet.asPercentDiff(
                    BalancedDietModSystem.serverConfig.oneFoodCategorySatietyMultiplier),
                BalancedDiet.asPercentDiff(
                    BalancedDietModSystem.serverConfig.twoFoodCategorySatietyMultiplier),
                BalancedDiet.asPercentDiff(
                    BalancedDietModSystem.serverConfig.threeFoodCategorySatietyMultiplier)
            );

            comps = VtmlUtil.Richtextify(capi, Text, CairoFont.WhiteSmallText().WithLineHeightMultiplier(1.2));
            titleCached = Lang.Get(Title).ToSearchFriendly();
        }

        public override void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor)
        {
            detailViewGui.AddRichtext(comps, textBounds, "richtext");
        }

        public void Recompose(ICoreClientAPI capi)
        {
            Texture?.Dispose();
            Texture = new TextTextureUtil(capi).GenTextTexture(Lang.Get(Title), CairoFont.WhiteSmallText());
        }

        public override PageText GetPageText()
        {
            return new PageText
            {
                Title = titleCached,
                Text = Text
            };
        }

        public override void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWidth, double cellHeight)
        {
            float num = (float)GuiElement.scaled(25.0);
            float num2 = (float)GuiElement.scaled(10.0);
            if (Texture == null)
            {
                Recompose(capi);
            }

            capi.Render.Render2DTexturePremultipliedAlpha(Texture.TextureId, x + (double)num2, y + (double)(num / 4f) - GuiElement.scaled(3.0), Texture.Width, Texture.Height);
        }
    }
}
