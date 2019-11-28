using QATool.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;

namespace QATool.Tests
{
    public class AccuracyPotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.TryEnterGame(client, 0);

            InsertPotionInSlot(client, 130, 23); //130_accuracy_potion   23 slot

            client.GoToTestLevel();
            
            CallTarget(client);
            ChangeTarget(client);
            client.EnterConsoleCheat("/infltarget 3 1000 1000"); // реген

            Logger.Instance.Send("проверим сколько критов без зелья");
            int critCountBefore = 0;
            for (int i = 0; i < 10; i++)
            {
                client.ClickSkill(0, 2);
                bool isCrit = client.inspectorData.Exist("TextFrame(Clone).Content.CriticalHitBg");
                if (isCrit)
                {
                    critCountBefore++;
                }
                Logger.Instance.Send($"{i} удар = " + (isCrit ? "крит" : "не крит"));
            }
            Logger.Instance.Send("теперь выпьем зелье и посмотрим сколько критов выпадет");
            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(2);


            int critCountAfter = 0;
            for (int i = 0; i < 10; i++)
            {
                client.ClickSkill(0, 2);
                bool isCrit = client.inspectorData.Exist("TextFrame(Clone).Content.CriticalHitBg");
                if (isCrit)
                {
                    critCountAfter++;
                }
                Logger.Instance.Send($"{i} удар = " + (isCrit ? "крит" : "не крит"));
            }
            Logger.Instance.Send($"Количество критов до: {critCountBefore}; после: {critCountAfter}");
            if (critCountAfter > critCountBefore)
                Acceptable("заебись");
            else
                Fail("хуёво");

            Logger.Instance.Send("ещё раз бахним пельменей и посмотрим сколько хп и маны даёт зелье");

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
            


            ExitToLobby(client);

        }

        
    }
}


