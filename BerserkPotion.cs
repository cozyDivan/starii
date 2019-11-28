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
    public class BerserkPotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            InsertPotionInSlot(client, 128, 21); //128_berserk_potion   21 slot

            client.GoToTestLevel();
            
            client.EnterConsoleCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 50");
            client.EnterConsoleCheat("/statadd 8865 SpAdd", "/statadd -1 SafeZone", "/sp 50");

            client.EnterConsoleCheat("/pos 40 40", "/call target 0");
            client.WaitShowElementGUIAndMouseClick("ControlsPanel.NextAimButton", 1);
            client.EnterConsoleCheat("/infltarget 3 1000 1000");
            int dmgWhiteAttack = client.GetDamage(0, 2);
            Logger.Instance.Send("чистенький урон белой атакой = " + dmgWhiteAttack);

            //используем зелье и смотрим как оно работают
            double hpBeforeUsePotion = client.GetHp(0, 1);
            double manaBeforeUsePotion = client.GetMana(0, 1);

            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(2);
            int dmgWhiteAttackWithPotion = client.GetDamage(0, 2);
            Logger.Instance.Send("урон белой атакой под зельем = " + dmgWhiteAttackWithPotion);
            Logger.Instance.Send("подождём 10 сек, пока бафф спадёт");
            Wait(8); 

            double hpAfterUsePotion = client.GetHp(0, 1);
            double manaAfterUsePotion = client.GetMana(0, 1);
            Logger.Instance.Send($"хп было = {hpBeforeUsePotion}, хп в зелье = {Constants.hpFromRechargePotion}, хп стало = {hpAfterUsePotion}");
            Logger.Instance.Send($"маны было = {manaBeforeUsePotion}, маны в зелье = {Constants.mpFromRechargePotion}, маны стало = {manaAfterUsePotion}");

            if (Math.Abs(hpBeforeUsePotion + Constants.hpFromRechargePotion - hpAfterUsePotion) > 1 && Math.Abs(manaBeforeUsePotion + Constants.mpFromRechargePotion - manaAfterUsePotion) > 1)
                Fail("Зелье Берсерка - не правильно начисляет хп / ману");
            else
                Acceptable("Зелье Берсерка - хп/мана - ок");


            if (Math.Abs(dmgWhiteAttack + Constants.dmgUpFromBerserkPotion * dmgWhiteAttack - dmgWhiteAttackWithPotion) > 1)
                Fail("Зелье Берсерка не увеличивает урон");
            else
                Acceptable("Зелье Берсерка - урон - ок");

            Logger.Instance.Send("проверим что зелье перестало работать");
            int dmgWhiteAttackWithoutPotion = client.GetDamage(0, 2);

            if (dmgWhiteAttackWithoutPotion != dmgWhiteAttack)
                Fail("Зелье Берсерка действует слишком долго");
            
            ExitToLobby(client);

        }

        
    }
}


