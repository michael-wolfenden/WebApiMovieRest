using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace WebApiMovieRest.Infrastructure.Tests.Helpers
{
    public static class Extensions
    {
        public static T ContentAs<T>(this HttpResponseMessage self)
        {
            T content;
            self.TryGetContentValue(out content);

            return content;
        }

        public static ModelStateDictionary ReconstructModelState(this HttpError httpError)
        {
            const string MODEL_STATE_KEY = "ModelState";

            if (!httpError.ContainsKey(MODEL_STATE_KEY)) return null;

            var reconstructedModelState = new ModelStateDictionary();
            var modelStateErrors = (HttpError) httpError[MODEL_STATE_KEY];

            foreach (var modelStateError in modelStateErrors)
            {
                var key = modelStateError.Key;
                var errorMessages = (string[]) modelStateError.Value;

                foreach (var errorMessage in errorMessages)
                {
                    reconstructedModelState.AddModelError(key, errorMessage);
                }
            }

            return reconstructedModelState;
        }

        public static string ErrorMessageForKey(this ModelStateDictionary modelState, string key)
        {
            return modelState[key].Errors
                                  .Single()
                                  .ErrorMessage;
        }
    }
}