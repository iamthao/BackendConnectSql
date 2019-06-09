using System.Collections.Generic;
using AutoMapper;
using Framework.DomainModel.Entities;
using Framework.Utility;

namespace QuickspatchWeb.Models.Mapping
{
    public class GridConfigMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<GridConfig, GridConfigViewModel>()
                .AfterMap(
                    (s, d) =>
                    {
                        d.ViewColumns = SerializationHelper.Deserialize<List<ViewColumnViewModel>>(s.XmlText);
                        if (d.ViewColumns != null)
                        {
                            d.ViewColumns.ForEach(c => c.Text = c.Text ?? string.Empty);
                        }
                        //enable by default, later may come back to get from config
                        d.AllowReorderColumn = true;
                        d.AllowResizeColumn = true;
                        d.AllowShowHideColumn = true;
                    });

            Mapper.CreateMap<GridConfigViewModel, GridConfig>()
                .ForMember(x => x.XmlText, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .AfterMap((s, d) => d.XmlText = SerializationHelper.SerializeToXml(s.ViewColumns));
        }
    }
}