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
    public class RecoveryPotions : Potion
    {
        public override void Run()
        {
            Client client = GetClient(0);

            client = GetClient(0);

            client.EnterCheat("/delall");
            Navigation.EnterGame(client, 0);

            Wait(() => client.inspectorData.Exist("ChangePersonButtonHolder.LobbyMenuButtonFrame(Clone)"), 10);
            client.EnterCheat("/getitem 121 3"); // большое зелье восстановления
            client.EnterCheat("/level 10", "/goldadd 100000", "/emerald 100000");

            //идем покупать зелья!
            BuyPotion(client, 3, 13, 0); // малое зелье восстановления
            BuyPotion(client, 3, 14, 1); // среднее зелье восстановления
            client.WaitShowElementGUIAndMouseClick("ItemFramePW3(Clone)", 15, 3); // большое зелье восстановления
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("PotionSlotsHolder.ItemFramePW3(Clone)", 2, 3); // 3й слот в поясе
            Wait(1);
            client.WaitShowElementGUIAndMouseClick("ButtonBack.Arrow", 1); // назад
            Wait(1);

            client.GoToTestLevel();
            client.EnterCheat("/statadd 8225 HpAdd", "/statadd -150 HpRegenAdd", "/hp 1");
            client.EnterCheat("/statadd 8865 SpAdd", "/statadd -1 SafeZone", "/sp 1");

            //используем зелья и смотрим сколько хп они восстанавливают
            UseRegenerationAndEquale(client, 3, 3);
            client.EnterCheat("/hp 1", "/sp 1");
            UseRegenerationAndEquale(client, 2, 4);
            client.EnterCheat("/hp 1", "/sp 1");
            UseRegenerationAndEquale(client, 1, 5);

            //используем зелья и смотрим что количество уменьшилось на 1
            int oldFourthPotionCount = GetPotionCount(client, 3);
            int oldThirdPotionCount = GetPotionCount(client, 2);
            int oldSecondPotionCount = GetPotionCount(client, 1);

            UsePotionBySlotId(client, 3);
            UsePotionBySlotId(client, 2);
            UsePotionBySlotId(client, 1);

            bool FourthPotionCheck = oldFourthPotionCount - GetPotionCount(client, 3) == 1;
            bool ThirdPotionCheck = oldThirdPotionCount - GetPotionCount(client, 2) == 1;
            bool SecondPotionCheck = oldSecondPotionCount - GetPotionCount(client, 1) == 1;



            ExitToLobby(client);

        }


       
    }
}

