﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack.model
{
  class Dealer : Player
  {
    private Deck m_deck = null;
    private const int g_maxScore = 21;

    private rules.INewGameStrategy m_newGameRule;
    private rules.IHitStrategy m_hitRule;

    private rules.IWinnerStrategy m_winRule;

    public Dealer(rules.RulesFactory a_rulesFactory)
    {
      m_newGameRule = a_rulesFactory.GetNewGameRule();
      m_hitRule = a_rulesFactory.GetHitRule();
      m_winRule = a_rulesFactory.GetWinnerRule();
    }

    public bool NewGame(Player a_player)
    {
      if (m_deck == null || IsGameOver())
      {
        m_deck = new Deck();
        ClearHand();
        a_player.ClearHand();
        return m_newGameRule.NewGame(m_deck, this, a_player);
      }
      return false;
    }

    public bool Hit(Player a_player)
    {
      if (m_deck != null && m_hitRule.DoHit(this) && !IsGameOver())
      {
        DealHand(a_player);
        return true;
      }
      return false;
    }

    public bool IsDealerWinner(Player a_player)
    {
      if (a_player.CalcScore() > g_maxScore)
      {
        return true;
      }
      else if (CalcScore() > g_maxScore)
      {
        return false;
      }
      else if(CalcScore() > a_player.CalcScore() &&  CalcScore() <= g_maxScore)
      {
        return true;
      } 
      else
      {
        return m_winRule.isWinner(CalcScore(), a_player.CalcScore());
      } 
    }

    public bool IsGameOver()
    {
        if (m_deck != null && /*CalcScore() >= g_hitLimit*/ m_hitRule.DoHit(this) != true)
        {
            return true;
        }
        return false;
    }

    public bool Stand()
    {
        if (m_deck != null)
        {
            ShowHand();
            while (m_hitRule.DoHit(this))
            {
                DealHand(this);
            }
            return true;
        }
        return false;
    }

    public void DealHand(Player player)
    {
        Card c;
        c = m_deck.GetCard();
        c.Show(true);
        player.DealCard(c);
    }
  }
}