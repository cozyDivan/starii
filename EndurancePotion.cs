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
    public class EndurancePotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.TryEnterGame(client, 0);

            InsertPotionInSlot(client, 133, 26); // 133_strength_potion   26 slot

            client.GoToTestLevel();

            Logger.Instance.Send("Вешаю на себя инфл наносящий урон, накачиваюсь хп шоб не умереть");
            client.EnterConsoleCheat("/statadd 10000 HpAdd", "/hp 100", "/infl 71 15 100"); // горение

            int damageOriginal = client.GetDamage(out bool crit, 1);
            Logger.Instance.Send($"урон по мне до употребления пельмененй - {damageOriginal}");

            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(3);

            int damageChanged = client.GetDamage(out _, 1);
            Logger.Instance.Send($"урон по мне после употребления пельмененй - {damageChanged}");

            AreEqual(damageOriginal * Constants.ArmorFromEndurancePotion, damageChanged, 1);
            
            client.EnterConsoleCheat("/hp 100", "/statadd -10000 HpAdd");


            

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

            
            
            ExitToLobby(client);

        }

        
    }
}


