using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsManager
{
    public static event Action<bool> OnShowPlayerOwnedOutpostsEverywhereChanged;
    public static event Action<bool> OnHideDeadOutpostsChanged;
    public static event Action<bool> OnHideOtherPlayersOutpostChanged;

    private static bool _showPlayerOwnedOutpostsEverywhere;
    private static bool _hideDeadOutposts = true;
    private static bool _hideOtherPlayersOutpost;

    public static bool ShowPlayerOwnedOutpostsEverywhere
    {
        get => _showPlayerOwnedOutpostsEverywhere;
        set
        {
            if (_showPlayerOwnedOutpostsEverywhere != value)
            {
                _showPlayerOwnedOutpostsEverywhere = value;
                OnShowPlayerOwnedOutpostsEverywhereChanged?.Invoke(value);
            }
        }
    }

    public static bool HideDeadOutposts
    {
        get => _hideDeadOutposts;
        set
        {
            if (_hideDeadOutposts != value)
            {
                _hideDeadOutposts = value;
                OnHideDeadOutpostsChanged?.Invoke(value);
            }
        }
    }

    public static bool HideOtherPlayersOutpost
    {
        get => _hideOtherPlayersOutpost;
        set
        {
            if (_hideOtherPlayersOutpost != value)
            {
                _hideOtherPlayersOutpost = value;
                OnHideOtherPlayersOutpostChanged?.Invoke(value);
            }
        }
    }

}
