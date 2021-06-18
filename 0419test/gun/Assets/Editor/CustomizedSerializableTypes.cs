using Cheetah.Common.SerializeTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public static class CustomizedSerializableTypes
    {

        [SupportedComponentType]
        static SupportedTypeData DefineTypeLongPressEventTrigger()
        {
            return new SupportedTypeData(typeof(LongPressEventTrigger), 200, null, null, null, null);
        }

        [SupportedComponentType]
        static SupportedTypeData DefineTabGroup()
        {
            return new SupportedTypeData(typeof(TabGroup), 100, null, null, null, null);
        }

        [SupportedComponentType]
        static SupportedTypeData DefineNewbieGuideButton()
        {
            return new SupportedTypeData(typeof(NewbieGuideButton), 300, null, null, null, null);
        }

        [SupportedComponentType]
        static SupportedTypeData DefineInputSlider()
        {
            return new SupportedTypeData(typeof(InputSlider), 400, null, null, null, null);
        }

        [SupportedComponentType]
        static SupportedTypeData DefineLanguageTip()
        {
            return new SupportedTypeData(typeof(LanguageTip), 500, null, null, null, null);
        }

        [SupportedComponentType]
        static SupportedTypeData DefineBubbleTip()
        {
            return new SupportedTypeData(typeof(BubbleTip), 600, null, null, null, null);
        }

    }
}
