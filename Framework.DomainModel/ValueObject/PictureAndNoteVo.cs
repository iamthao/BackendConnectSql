using System;
using System.Globalization;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class PictureAndNoteVo: ReadOnlyGridVo
    {
        public string Picture { get; set; }
        public string Note { get; set; }
    }
}