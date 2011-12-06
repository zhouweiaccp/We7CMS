using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    public enum QuestionType
    {
        Radio = 0,
        RadioOther = 1,
        CheckBox = 2,
        CheckBoxOther = 3,
        Select = 4,
        ShortAnswer = 5,
        Question = 6,
        Blanks = 7,
        Custom = 8
    }

    public enum ContentType
    {
        /// <summary>
        /// <input type="radio"></input>
        /// </summary>
        Radio = 0,

        /// <summary>
        /// <input type="checkbox"></input>
        /// </summary>
        CheckBox = 1,

        /// <summary>
        /// <select></select>
        /// </summary>
        Select = 2,

        /// <summary>
        /// <input type="text"></input>
        /// </summary>
        Text = 4,

        /// <summary>
        /// <textarea></textarea>
        /// </summary>
        Textarea = 8
    }
}
