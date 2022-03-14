using Cofoundry.Domain;
using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;

namespace CofoundryExample.SSOAndContent
{
    public class SimpleContentRepository
    {
        private readonly IContentRepository _contentRepository;

        public SimpleContentRepository(
            IContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        public async Task<IHtmlContent> GetContentByKeyAsync(string contentKey)
        {
            var content = await _contentRepository
                .CustomEntities()
                .GetByUrlSlug<SimpleContentCustomEntityDefintion>(contentKey)
                .AsRenderSummary(PublishStatusQuery.Published)
                .Map(entity =>
                {
                    var model = (SimpleContentDataModel)entity.Model;
                    return new HtmlString(model.Html);
                })
                .ExecuteAsync();

            return content;
        }
    }
}