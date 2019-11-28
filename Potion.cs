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
    public abstract class Potion : AutoTest
    {
        public void UsePotionAndEqualHp(Client client, int potionSlotId, int potionSize)
        {
            double HPfromPotion = 0d;
            switch (potionSize)
            {
                case 0:
                    HPfromPotion = 150; //Малое лечебное зелье
                    break;
                case 1:
                    HPfromPotion = 300; //Среднее
                    break;
                case 2:
                    HPfromPotion = 500; //Сильное 
                    break;
                case 3:
                    HPfromPotion = 1000; //Большое 
                    break;
                case 4:
                    HPfromPotion = 2500; //Чемпионское 
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            string potionName = string.Empty;
            switch (potionSize)
            {
                case 0:
                    potionName = "Малое лечебное";
                    break;
                case 1:
                    potionName = "Среднее лечебное";
                    break;
                case 2:
                    potionName = "Сильное лечебное";
                    break;
                case 3:
                    potionName = "Большое лечебное";
                    break;
                case 4:
                    potionName = "Чемпионское лечебное";
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            double hpBeforeUsePotion = client.GetHp(0, 1);
            client.WaitShowElementGUIAndMouseClick($"Potion{potionSlotId}Holder", 1, yDiff: 10);
            Wait(1);
            //switch (potionSize)
            //{
            //    case 0:
            //    case 1:
            //    case 2:
            //        Wait(12);
            //        break;
            //    case 3:
            //    case 4:
            //        Wait(22);
            //        break;
            //    default:
            //        Logger.Instance.Send("Нет бутылочки такого размера");
            //        break;
            //}

            double hpAfterUsePotion = client.GetHp(0, 1);
            Logger.Instance.Send($"было хп = {hpBeforeUsePotion}, хп в зелье = {HPfromPotion}, хп стало = {hpAfterUsePotion}");
            if (Math.Abs(hpBeforeUsePotion + HPfromPotion - hpAfterUsePotion) > 1)
            {
                Fail(potionName + " - поломалось");
            }
            else
                Acceptable(potionName + " - ок");

        }

        public void BuyPotion(Client client, int clickPlusCount, int elementNumberBackPack, int beltPosition)
        {
            if (client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)")) // лобби
            {
                client.WaitShowElementGUIAndMouseClick("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)", 3); // арсенал
                Wait(0.5);
                client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 3, 3); // зелья
                Wait(0.5);
            }
            client.WaitShowElementGUIAndMouseClick("ItemFramePW3(Clone)", elementNumberBackPack, 3);
            Wait(0.5);
            if (client.inspectorData.Exist("NotificationForm"))
            {
                client.WaitShowElementGUIAndMouseClick("ButtonBack", 1);
                Wait(0.5);
            }

            for (int i = 0; i < clickPlusCount; i++)
            {
                client.WaitShowElementGUIAndMouseClick("BtnCountPlusIcon", 1);
                Wait(0.5);
            }
            client.WaitShowElementGUIAndMouseClick("BtnBuy.PricePanel", 1);
            Wait(0.5);
            client.WaitShowElementGUIAndMouseClick("ButtonOkText", 3);
            Wait(0.5);
            client.WaitShowElementGUIAndMouseClick("PotionSlotsHolder.ItemFramePW3(Clone)", beltPosition, 3); // слот в поясе
            Wait(0.5);
        }

        public void ExitToLobby(Client client)
        {
            client.WaitShowElementGUIAndMouseClick("MainMenuIcon", 1);
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeaveBattleButton", 1, 0, -30);
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ExitButton.Text", 1);
            Wait(1);
            if (client.inspectorData.Exist("SpecialOfferFormPW3"))
            {
                client.WaitShowElementGUIAndMouseClick("SpecialOfferFramePW3(Clone).Content.CloseButton.CloseButtonIcon", 2, 1);
            }
        }

        public int GetPotionCount(Client client, int potionSlotId)
        {
            string potionSlotButton = $"Potion{potionSlotId}Holder.LocationPotionFrame(Clone).PotionCountBack.PotionCountText";
            if (client.inspectorData.Exist(potionSlotButton))
            {
                string textPotionSlotCount = client.inspectorData.GetText(potionSlotButton);
                int valuePotionSlotCount = int.Parse(textPotionSlotCount);
                return valuePotionSlotCount;
            }
            else
            {
                Fail($"Не найден элемент {potionSlotButton}");
            }
            return 0;
        }

        public void UsePotionBySlotId(Client client, int potionSlotId)
        {
            client.WaitShowElementGUIAndMouseClick($"Potion{potionSlotId}Holder", 2, yDiff: 10);

        }

        public void UseManaAndEquale(Client client, int potionSlotId, int potionSize)
        {
            double SPfromPotion = 0d;
            switch (potionSize)
            {
                case 0:
                    SPfromPotion = 100; //Малое лечебное зелье
                    break;
                case 1:
                    SPfromPotion = 200; //Среднее
                    break;
                case 2:
                    SPfromPotion = 400; //Сильное 
                    break;
                case 3:
                    SPfromPotion = 750; //Большое 
                    break;
                case 4:
                    SPfromPotion = 1500; //Чемпионское 
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            string potionName = string.Empty;
            switch (potionSize)
            {
                case 0:
                    potionName = "Малое зелье маны";
                    break;
                case 1:
                    potionName = "Среднее зелье маны";
                    break;
                case 2:
                    potionName = "Сильное зелье маны";
                    break;
                case 3:
                    potionName = "Большое зелье маны";
                    break;
                case 4:
                    potionName = "Чемпионское зелье маны";
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            double manaBeforeUsePotion = client.GetMana(0, 1);
            client.WaitShowElementGUIAndMouseClick($"Potion{potionSlotId}Holder", 1, yDiff: 10);
            Wait(1);

            double manaAfterUsePotion = client.GetMana(0, 1);
            Logger.Instance.Send($"маны было = {manaBeforeUsePotion}, маны в зелье = {SPfromPotion}, маны стало = {manaAfterUsePotion}");
            if (Math.Abs(manaBeforeUsePotion + SPfromPotion - manaAfterUsePotion) > 1)
            {
                Fail(potionName + " - поломалось");
            }
            else
                Acceptable(potionName + " - ок");

        }
        public void UseRegenerationAndEquale(Client client, int potionSlotId, int potionSize)
        {
            double hpFromPotion = 0d;
            double SPfromPotion = 0d;
            switch (potionSize)
            {
                case 0:
                    hpFromPotion = 300 + (client.GetHp(1, 1) * 0.03) * 5;
                    SPfromPotion = 100 + (client.GetMana(1, 1) * 0.01) * 5; // Малое
                    break;
                case 1:
                    hpFromPotion = 500 + (client.GetHp(1, 1) * 0.05) * 5;
                    SPfromPotion = 200 + (client.GetMana(1, 1) * 0.03) * 5; // среднее
                    break;
                case 2:
                    hpFromPotion = 1000 + (client.GetHp(1, 1) * 0.10) * 5;
                    SPfromPotion = 400 + (client.GetMana(1, 1) * 0.05) * 5; // большое
                    break;
                case 3:
                    hpFromPotion = 150 + (client.GetHp(1, 1) * 0.01) * 5;
                    SPfromPotion = 200 + (client.GetMana(1, 1) * 0.03) * 5; // Малое
                    break;
                case 4:
                    hpFromPotion = 300 + (client.GetHp(1, 1) * 0.03) * 5;
                    SPfromPotion = 400 + (client.GetMana(1, 1) * 0.05) * 5; // среднее
                    break;
                case 5:
                    hpFromPotion = 500 + (client.GetHp(1, 1) * 0.05) * 5;
                    SPfromPotion = 750 + (client.GetMana(1, 1) * 0.10) * 5; // большое
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            string potionName = string.Empty;
            switch (potionSize)
            {
                case 0:
                    potionName = "Малое зелье регенерации";
                    break;
                case 1:
                    potionName = "Среднее зелье регенерации";
                    break;
                case 2:
                    potionName = "Большое зелье регенерации";
                    break;
                case 3:
                    potionName = "Малое зелье восстановления";
                    break;
                case 4:
                    potionName = "Среднее зелье восстановления";
                    break;
                case 5:
                    potionName = "Большое зелье восстановления";
                    break;
                default:
                    Logger.Instance.Send("Нет бутылочки такого размера");
                    break;
            }

            double hpBeforeUsePotion = client.GetHp(0, 1);
            double manaBeforeUsePotion = client.GetMana(0, 1);
            client.WaitShowElementGUIAndMouseClick($"Potion{potionSlotId}Holder", 1, yDiff: 10);
            Wait(6); // время действия зелья

            double hpAfterUsePotion = client.GetHp(0, 1);
            double manaAfterUsePotion = client.GetMana(0, 1);
            Logger.Instance.Send($"хп было = {hpBeforeUsePotion}, хп в зелье = {hpFromPotion}, хп стало = {hpAfterUsePotion}");
            Logger.Instance.Send($"маны было = {manaBeforeUsePotion}, маны в зелье = {SPfromPotion}, маны стало = {manaAfterUsePotion}");

            if (Math.Abs(hpBeforeUsePotion + hpFromPotion - hpAfterUsePotion) > 1 && Math.Abs(manaBeforeUsePotion + SPfromPotion - manaAfterUsePotion) > 1)
            {
                Fail(potionName + " - поломалось");
            }
            else
                Acceptable(potionName + " - ок");

        }

        public void CheckRemoveBuff(Client client, string[] infls, int potionSlot, int hpInPotion)
        {
            foreach (string s in infls)
            {
                client.EnterCheat(s);
            }

            if (hpInPotion != 500)
            {
                if (client.inspectorData.GetCount("BuffFrame(Clone)") != 1)
                    Fail("неверное количество баффов");
                else
                    Acceptable("верное количество баффов");
            }
            else
            {
                if (client.inspectorData.GetCount("BuffFrame(Clone)") != 4)
                    Fail("неверное количество баффов");
                else
                    Acceptable("верное количество баффов");
            }

            double hpBeforeUsePotion = client.GetHp(0, 1);
            client.WaitShowElementGUIAndMouseClick($"Potion{potionSlot}Holder", 1, yDiff: 10);
            Wait(1);
            double hpAfterUsePotion = client.GetHp(0, 1);
            Logger.Instance.Send($"хп было = {hpBeforeUsePotion}, хп в зелье = {hpInPotion}, хп стало = {hpAfterUsePotion}");

            if (Math.Abs(hpBeforeUsePotion + hpInPotion - hpAfterUsePotion) > 1)
            {
                Fail(" - хп поломалось");
            }
            else
                Acceptable(" - хп ок");

            if (client.inspectorData.GetCount("BuffFrame(Clone)") != 0)
                Fail("бафф не снялся");
            else
                Acceptable("зелье сняло бафф");
        }

        public void InsertPotionInSlot(Client client, int potionId, int potionSlotInBackpack)
        {
            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);
            client.EnterCheat($"/getitem {potionId} 3", "/level 10");

            //идем покупать зелья!
            client.WaitShowElementGUIAndMouseClick("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)", 3); // арсенал
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("LeftMenuButtonFramePW3(Clone)", 3, 3); // зелья
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ItemFramePW3(Clone)", potionSlotInBackpack, 3);
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("PotionSlotsHolder.ItemFramePW3(Clone)", 0, 3); // 1й слот в поясе
            Wait(1);

            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            Wait(1);
        }

        public void ChangeTarget(Client client)
        {
            client.WaitShowElementGUIAndMouseClick("ControlsPanel.NextAimButton", 1);
        }

        public void CallTarget(Client client)
        {
            client.EnterCheat("/pos 40 40", "/call target 0");
        }

    }
}

