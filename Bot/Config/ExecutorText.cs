using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Config
{
    public static class ExecutorText
    {
        public static InfoExecutorText InfoExecutor { get; private set; } = new InfoExecutorText();
        public static string CantPermission = "Вы не имеете права на использование данной команды!";
        public static string ExepctionText = "Что-то пошло не так! Проверьте правильность параметров команды.";
        public static AddHomeWorkExecutorText AddHomeWorkExecutor { get; private set; } = new AddHomeWorkExecutorText();
        public static CheckHomeWorkExecutorText CheckHomeWorkExecutor { get; private set; } = new CheckHomeWorkExecutorText();
        public static DeleteHomeWorkExecutorText DeleteHomeWorkExecutor { get; private set; } = new DeleteHomeWorkExecutorText();
    }
    public class InfoExecutorText
    {
        public string InfoDescription = "Список доступных команд:\n\n";
    }
    public class AddHomeWorkExecutorText
    {
        public string CantAddedTwix = "Вы не можете добавить домашнее задание на один и тот же день";
        public string ParamDateError = "Вы не вверли необходимый параметер(дату домашнего задания)";
        public string ParamTextError = "Вы не вверли необходимый параметер(текст домашнего задания)";
    }
    public class CheckHomeWorkExecutorText
    {
        public string HomeWorkNull = "Извините, но список домашнего задания пуст!";
        public string HomeWorkDoesntAdded = "Извините, но дз ещё не добавлено!";
    }
    public class DeleteHomeWorkExecutorText
    {
        public string ErrorDelete = "Вы не можете удалить несуществуещее домашнее задание";
        public string SuccessDelete = "Домашнее задание успешно удалено!";
    }
    //public class HomeWorkExecutorHelperText
    //{
    //    public string CantAddedTwix="Вы не можете установить домашнее задание на выходные";
    //    public string SuccessAdded = "Домашнее задание успешно добавлено на дату";
    //}
}
