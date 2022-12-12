using TMPro;
using UnityEngine;

namespace Leaderboards
{
    public class Leader : MonoBehaviour
    {
        [SerializeField] private TMP_Text loginText;
        [SerializeField] private TMP_Text recordText;

        public void Init(int place, string login, int record)
        {
            loginText.text = $"{place}. {login}";
            recordText.text = $"{record}";
        }
    }
}