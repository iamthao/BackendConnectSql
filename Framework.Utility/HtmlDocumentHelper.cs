using HtmlAgilityPack;

namespace Framework.Utility
{
    public class HtmlDocumentHelper
    {
        private readonly HtmlDocument _document;
        public HtmlDocumentHelper(string contentHtml)
        {
            _document = new HtmlDocument();
            _document.LoadHtml(contentHtml);
        }

        public string GetContentById(string id)
        {
            var note = _document.GetElementbyId(id);
            if (note != null)
                return note.InnerHtml;
            return string.Empty;
        }

        public string RemoveContentById(string id)
        {
            var note = _document.DocumentNode.SelectSingleNode("//div[@id='" + id + "']");
            if (note != null)
                note.ParentNode.RemoveChild(note);
            return _document.DocumentNode.InnerHtml;
        }

        public string MergeUrlOnImage(string url)
        {
            var notes = _document.DocumentNode.SelectNodes("//img");
            if (notes != null && notes.Count > 0)
            {
                foreach (var note in notes)
                {
                    note.Attributes["src"].Value = url + note.Attributes["src"].Value;
                }
            }
            return _document.DocumentNode.InnerHtml;
        }
    }
}
