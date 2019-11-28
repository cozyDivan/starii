using QATool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;

namespace QATool.Tests
{
    public class AgilityPotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.TryEnterGame(client, 0);

            InsertPotionInSlot(client, 131, 24); //131_dexterity_potion   24 slot

            client.GoToTestLevel();

            client.EnterConsoleCheat("/getstat ASPD"); // скорость атаки
            client.WaitShowElementGUIAndMouseClick("TopContainer.ChatButton.Image", 1);
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 5, 1);
            Wait(1);
            int chatMessageCount = client.GetCount("PlayerUserShortMessageFrame(Clone)");
            double aspd = client.GetValue("PlayerUserShortMessageFrame(Clone).Content.MessageText", chatMessageCount - 1, 2);
            Logger.Instance.Send("скорость атаки =" + aspd);
            client.WaitShowElementGUIAndMouseClick("ButtonBack.ButtonImage", 3);

            Logger.Instance.Send("бахним пельменей и посмотрим сколько хп и маны даёт зелье");

            client.EnterConsoleCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 50");
            client.EnterConsoleCheat("/statadd 8865 SpAdd", "/statadd -1 SafeZone", "/sp 50");

            double hpBeforeUsePotion = client.GetHp(0, 1);
            double manaBeforeUsePotion = client.GetMana(0, 1);

            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(2);

            double hpAfterUsePotion = client.GetHp(0, 1);
            double manaAfterUsePotion = client.GetMana(0, 1);
            Logger.Instance.Send($"хп было = {hpBeforeUsePotion}, хп в зелье = {Constants.hpFromRechargePotion}, хп стало = {hpAfterUsePotion}");
            Logger.Instance.Send($"маны было = {manaBeforeUsePotion}, маны в зелье = {Constants.mpFromRechargePotion}, маны стало = {manaAfterUsePotion}");

            if (Math.Abs(hpBeforeUsePotion + Constants.hpFromRechargePotion - hpAfterUsePotion) > 1 && Math.Abs(manaBeforeUsePotion + Constants.mpFromRechargePotion - manaAfterUsePotion) > 1)
                Fail("Зелье - не правильно начисляет хп / ману");
            else
                Acceptable("Зелье - хп/мана - ок");


            client.EnterConsoleCheat("/getstat ASPD"); // скорость атаки
            client.WaitShowElementGUIAndMouseClick("TopContainer.ChatButton.Image", 1);
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 5, 1);
            Wait(1);
            int chatMessageCountAfter = client.GetCount("PlayerUserShortMessageFrame(Clone)");
            double aspdAfter = client.GetValue("PlayerUserShortMessageFrame(Clone).Content.MessageText", chatMessageCountAfter - 1, 2);
            Logger.Instance.Send("скорость атаки после пельменей =" + aspdAfter);
            client.WaitShowElementGUIAndMouseClick("ButtonBack.ButtonImage", 3);

            if (Math.Abs(aspd + Constants.attackSpeedUp - aspdAfter) > 0)
                Fail("скорость атаки плохо увеличилась");
            else
                Acceptable("скорость атаки под пельменями увеличилась как надо");

            //////////////////////////////////////////////////////////////////////////////////////////////
            client.EnterConsoleCheat("/pos 13 12");
            double speedBefore = client.SpeedMeasurement("archer(Clone)", 1);
            
            client.EnterConsoleCheat("/pos 13 12");
            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(2);
            double speedAfter = client.SpeedMeasurement("archer(Clone)", 1);

            if (Math.Abs(speedBefore * Constants.moveSpeedUp - speedAfter) > 0.2)
                Fail("зелье плохо сработало");
            else
                Acceptable("зелье выдало спиды");
            
            ExitToLobby(client);

        }

        
    }
}


