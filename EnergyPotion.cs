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
    public class EnergyPotion : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.TryEnterGame(client, 0);

            InsertPotionInSlot(client, 134, 27); // 134_energy_potion   27 slot

            client.GoToTestLevel();

            Logger.Instance.Send("отключаю реген маны и смотрю сколько маны тратит двойной выстрел");
            client.EnterConsoleCheat("/statadd -1 SafeZone"); // отключение регена маны

            double manaBeforeUseSkill = client.GetMana(0, 1);
            client.ClickSkill(1, 1);
            int coolDown = client.CatchCooldown(1, 3);
            Wait(coolDown);
            double manaAfterUseSkill = client.GetMana(0, 1);
            if (Math.Abs(manaBeforeUseSkill - Constants.manaCostFromDoubleStrafe - manaAfterUseSkill) > 1)
                Fail($"Двойной выстрел тратит неверное количество маны ({manaBeforeUseSkill - manaAfterUseSkill})");
            else
                Acceptable("трата маны до бахнув пильменив = " + (manaBeforeUseSkill - manaAfterUseSkill));

           
            client.WaitShowElementGUIAndMouseClick($"Potion3Holder", 1, yDiff: 10); // выпили поушен
            Wait(2);


            double manaBeforeUseSkillUnderpotion = client.GetMana(0, 1);
            client.ClickSkill(1, 1);
            double manaAfterUseSkillUnderPotion = client.GetMana(0, 1);
            if (Math.Abs(manaBeforeUseSkillUnderpotion - Constants.manaCostFromDoubleStrafe * Constants.reduceManaCostFromUnderEnergyPotion - manaAfterUseSkillUnderPotion) > 1)
                Fail($"Двойной выстрел тратит неверное количество маны под зельем энергии ({manaBeforeUseSkillUnderpotion - manaAfterUseSkillUnderPotion})");
            else
                Acceptable("трата маны после бахнув пильменив = " + (manaBeforeUseSkillUnderpotion - manaAfterUseSkillUnderPotion));

            
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


