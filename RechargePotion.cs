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
    public class RechargePotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            InsertPotionInSlot(client, 132, 25); //132_recharge_potion   25 slot

            client.GoToTestLevel();
            
            client.EnterConsoleCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 50");
            client.EnterConsoleCheat("/statadd 8865 SpAdd", "/statadd -1 SafeZone", "/sp 50");

            client.ClickSkill(1, 2);
            int coolDownDefault = client.CatchCooldown(1,2);
            Wait(coolDownDefault);

            //используем зелье и смотрим как оно работают

            double hpBeforeUsePotion = client.GetHp(0, 1);
            double manaBeforeUsePotion = client.GetMana(0, 1);

            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10);
            client.ClickSkill(1, 2);
            int coolDownAfterUsePotion = client.CatchCooldown(1, 2);
            Wait(10);

            double hpAfterUsePotion = client.GetHp(0, 1);
            double manaAfterUsePotion = client.GetMana(0, 1);
            Logger.Instance.Send($"хп было = {hpBeforeUsePotion}, хп в зелье = {Constants.hpFromRechargePotion}, хп стало = {hpAfterUsePotion}");
            Logger.Instance.Send($"маны было = {manaBeforeUsePotion}, маны в зелье = {Constants.mpFromRechargePotion}, маны стало = {manaAfterUsePotion}");

            if (Math.Abs(hpBeforeUsePotion + Constants.hpFromRechargePotion - hpAfterUsePotion) > 1 && Math.Abs(manaBeforeUsePotion + Constants.mpFromRechargePotion - manaAfterUsePotion) > 1)
                Fail("Зелье Перезарядки - не правильно начисляет хп / ману");
            else
                Acceptable("Зелье Перезарядки - хп/мана - ок");

            if (coolDownDefault - Constants.reduceCooldownFromRechargePotion * coolDownDefault - coolDownAfterUsePotion > 0.1)
                Fail("Зелье Перезарядки не уменьшает кулдаун скилов");
            else
                Acceptable("Зелье Перезарядки - кулдаун - ок");

            Logger.Instance.Send("проверим что зелье перестало работать");
            client.ClickSkill(1, 2);
            int coolDownWithoutPotionInfl = client.CatchCooldown(1, 2);
            if (coolDownWithoutPotionInfl != coolDownDefault)
                Fail("Зелье Перезарядки действует слишком долго");
            
            ExitToLobby(client);

        }

        
    }
}


