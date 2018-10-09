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
using SearchBot.DependencyInjection;
using SearchBot.Dialogs;
using SearchBot.Translator;
using SearchBot.Utilities;
using Newtonsoft.Json;
using SearchBot.Extensions;

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

                    var dialog = scope.Resolve<LUISDialog>();
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
            var response = Request.CreateResponse(HttpStatusCode.OK,"Tested");
            return response;
        }
        private Activity HandleSystemMessage(Activity message)
        {
            string messageType = message.GetActivityType();
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

            return null;
        }
    }
}