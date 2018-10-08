using SearchBot.Translator;
using SearchBot.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace SearchBot.Extensions
{
    public static class StringExtensions
    {
        public static string ToUserLocale(this string text, IDialogContext context)
        {
            context.UserData.TryGetValue(StringConstants.UserLanguageKey, out string userLanguageCode);

            text = TranslationHandler.TranslateText(text, StringConstants.DefaultLanguage, userLanguageCode);

            return text;
        }
    }
}