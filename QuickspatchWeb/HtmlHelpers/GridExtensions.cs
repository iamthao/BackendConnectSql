using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using QuickspatchWeb.Models;

namespace QuickspatchWeb.HtmlHelpers
{
    public static class GridExtensions
    {

        public static MvcHtmlString GetGridViewSchemaConfigData(this HtmlHelper htmlHelper, GridViewModel viewModel)
        {
            var gridColumns = new StringBuilder();
            var schemas = new List<string>();

            gridColumns.Append("{");

            schemas.Add("\"Id\": { \"editable\": false }");
            foreach (var column in viewModel.ViewColumns)
            {
                var dataType = "";
                var formatString = string.IsNullOrEmpty(column.ColumnFormat)
                                         ? string.Empty
                                         : "format: '{" + column.ColumnFormat + "}', ";
                if (!string.IsNullOrEmpty(column.ColumnFormat)
                    && (column.ColumnFormat.Contains("yyyy") || column.ColumnFormat.ToLower().Contains("date")))
                {
                    dataType = ", \"type\":\"date\"";

                }
                schemas.Add("\"" + column.Name + "\": { \"editable\": false " + dataType + " }");

            }


            gridColumns.Append(string.Join(", \n", schemas.ToArray()));
            gridColumns.Append("}");

            return new MvcHtmlString(gridColumns.ToString());
        }

        public static MvcHtmlString GetGridColumnsConfigData(this HtmlHelper htmlHelper, GridViewModel viewModel)
        {
            var gridColumns = new StringBuilder();
            var columns = new List<string>();
            foreach (var column in viewModel.ViewColumns)
            {
                var columnWidthString = column.ColumnWidth == 0 ? "" : string.Format(", \"width\": {0}", column.ColumnWidth);
                var columnString = "{  \"field\": \"" + column.Name + "\", \"title\": \"" + column.Text + "\"" +
                                columnWidthString +
                                ", \"attributes\":{\"style\":\"text-align:" + column.ColumnJustification + ";\"}, \"sortable\": " + column.Sortable.ToString().ToLower() + ", \"hidden\": " + column.HideColumn.ToString().ToLower();
                if (!string.IsNullOrEmpty(column.CustomTemplate))
                {
                    columnString += ",\"template\":\"" + column.CustomTemplate + "\"";
                }
                if (!string.IsNullOrEmpty(column.ColumnFormat))
                {
                    columnString += ", \"format\": \"{" + column.ColumnFormat + "}\"";
                }
                columnString += "}";
                columns.Add(columnString);
            }
            // Check if user manager grid => add command reset pass and active
            if (viewModel.UseDeleteColumn)
            {
                // Add delete column
                const string delColumnString = "{\"field\": \"Command\", \"template\":\"commandTemplate\", \"title\": \"&nbsp;\", \"width\": \"150px\",\"mandatory\":true,\"sortable\":false}";
                columns.Add(delColumnString);
            }
            gridColumns.Append("[");
            gridColumns.Append(string.Join(", ", columns.ToArray()));
            gridColumns.Append("]");

            return new MvcHtmlString(gridColumns.ToString());
        }
    }
}