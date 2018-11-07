using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using SearchBot.Dialogs;
using SearchBot.Translator;
using SearchBot.Utilities;
using Newtonsoft.Json;
using SearchBot.Extensions;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace SearchBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 

        public MessagesController()
        {

        }
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            Trace.TraceInformation($"Incoming Activity is {activity.ToJson()}");
            //detect language of input text
            var userLanguage = TranslationHandler.DetectLanguage(activity);
            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                    var key = Address.FromActivity(activity);
                    var userData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, CancellationToken.None);
                    var storedLanguageCode = userData.GetProperty<string>(StringConstants.UserLanguageKey);

                    //update user's language in Azure Table Storage
                    if (storedLanguageCode != userLanguage)
                    {
                        userData.SetProperty(StringConstants.UserLanguageKey, userLanguage);
                        await botDataStore.SaveAsync(key, BotStoreType.BotUserData, userData, CancellationToken.None);
                        await botDataStore.FlushAsync(key, CancellationToken.None);
                    }   

                    //translate activity.Text to English before sending to LUIS for intent
                    activity.Text = TranslationHandler.TranslateTextToDefaultLanguage(activity, userLanguage);

                    var dialog = scope.Resolve<RootDialog>();
                    await Conversation.SendAsync(activity, () => dialog);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, "Tested");
            return response;
        }
        private Activity HandleSystemMessage(Activity activity)
        {
            string messageType = activity.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (messageType == ActivityTypes.Ping)
            {
            }
            else if(messageType == ActivityTypes.Invoke)
            {
                Trace.TraceInformation($"Inside invoke with activity name {activity.Name}");
                if (activity.Name == "signin/verifyState")
                {
                    // We do this so that we can pass handling to the right logic in the dialog. You can
                    // set this to be whatever string you want.
                    activity.Text = "loginComplete";
                    // Handle login completion event.
                    JObject ctx = activity.Value as JObject;

                    if (ctx != null)
                    {
                        string code = ctx["state"].ToString();
                        string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];
                        var oauthClient = activity.GetOAuthClient();
                        var token = oauthClient.OAuthApi.GetUserTokenAsync(activity.From.Id, ConnectionName, magicCode: code).Result;
                        //if (token != null)
                        //{
                           
                        //    // Make whatever API calls here you want
                        //    context.PostAsync($"Success! You are now signed in.");
                        //}
                    }


                }
            }

            return null;
        }
    }
}