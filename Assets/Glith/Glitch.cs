using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GlitchRenderer),PostProcessEvent.AfterStack,"Custom/Glitch")]
public sealed class Glitch : PostProcessEffectSettings
{
    [Range(0f,1f),Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter(){value = 0.5f};
    [Tooltip("Glitch Random Noise Texture")]
    public TextureParameter randomNoiseTexture = new TextureParameter();
    public FloatParameter blockSize = new FloatParameter();
    public FloatParameter floatGeneralValue = new FloatParameter();
    public FloatParameter lineColorShiftValueRed = new FloatParameter();
    public FloatParameter lineColorShiftValueGreen = new FloatParameter();
    public FloatParameter lineColorShiftValueBlue = new FloatParameter();
    public FloatParameter blockThrethIntensity = new FloatParameter();
    public FloatParameter lineThrethIntensity = new FloatParameter();
}

public sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
{
    private static readonly int Blend = Shader.PropertyToID("_Blend");
    private static readonly int RandomNoiseTexture = Shader.PropertyToID("_RandomNoiseTexture");
    private static readonly int BlockTiling = Shader.PropertyToID("_BlockSize");
    private static readonly int FloatGeneralValue = Shader.PropertyToID("_FloatGeneralValue0");
    private static readonly int LineColorShiftValueRed = Shader.PropertyToID("_LineColorShiftValueRed");
    private static readonly int LineColorShiftValueGreen = Shader.PropertyToID("_LineColorShiftValueGreen");
    private static readonly int LineColorShiftValueBlue = Shader.PropertyToID("_LineColorShiftValueBlue");
    private static readonly int BlockThrethIntensityNameID = Shader.PropertyToID("_BlockThrethIntensity");
    private static readonly int LineThrethIntensityNameID = Shader.PropertyToID("_LineThrethIntensity");

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));
        sheet.properties.SetFloat(Blend,settings.blend);
        sheet.properties.SetTexture(RandomNoiseTexture,settings.randomNoiseTexture);
        sheet.properties.SetFloat(BlockTiling,settings.blockSize);
        sheet.properties.SetFloat(FloatGeneralValue,settings.floatGeneralValue);
        sheet.properties.SetFloat(LineColorShiftValueRed,settings.lineColorShiftValueRed);
        sheet.properties.SetFloat(LineColorShiftValueGreen,settings.lineColorShiftValueGreen);
        sheet.properties.SetFloat(LineColorShiftValueBlue,settings.lineColorShiftValueBlue);
        sheet.properties.SetFloat(BlockThrethIntensityNameID,settings.blockThrethIntensity);
        sheet.properties.SetFloat(LineThrethIntensityNameID,settings.lineThrethIntensity);
        context.command.BlitFullscreenTriangle(context.source,context.destination,sheet,0);
    }
}