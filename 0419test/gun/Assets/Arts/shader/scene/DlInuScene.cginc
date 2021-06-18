// Copyright (C) 2018 haipeng li
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#include "UnityCG.cginc"
#include "AutoLight.cginc"

#define USE_RGBM defined(SHADER_API_MOBILE)

half CaculateClipValue(half3 pos, half4 scene_value, half interval, half progress)
{
    half dis = distance(pos, scene_value.xyz);
    return (interval * 2 - 1) * (dis / scene_value.w - progress-0.06305);
}

half3 CaculateClipColor(half interval, half clip,half4 color)
{
    return (interval*-1+1) * clamp(color.a-clip*10,0,1) * 10 * color.rgb;
}

half3 CaculateShadowColor(half atten)
{
    half temp = floor(atten);
    return atten*temp + (atten-temp)*unity_ShadowColor;
}

half CaculateGrayMoveClipValue(half3 pos, half4 scene_value, half interval, half progress)
{
    
    pos.z += sin(pos.x*0.5)*0.5;
    pos.x = 0;
    scene_value.x = 0;
    scene_value.y = 0;
    half dis = distance(pos, scene_value);
    return (interval * 2 - 1) * (dis / scene_value.w - progress*0.5);
}

half3 CaculateGrayMoveColor(half3 pos, half clip, half4 color)
{
    if(clip < 0){
        return (1-clamp(color.a-clip*10,0,1))*10* color.rgb;
    }else{
        return half3(0,0,0);
    }
}
