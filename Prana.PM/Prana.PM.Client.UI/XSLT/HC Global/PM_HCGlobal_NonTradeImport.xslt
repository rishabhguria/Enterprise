<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL12)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and COL5!='Buy' and COL5!='Sell' and COL5!='CoverShort' and COL5!='SellShort' and COL5!='Dividend'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Geneva'"/>
            </xsl:variable>

            <xsl:variable name = "varSubAccountDesc">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_PRE">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='AccountingRelated'">
                  <xsl:value-of select="'ACCTG'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Deposit'">
                  <xsl:value-of select="'CASH_DEP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='GrossAmountInterest' or $varSubAccountDesc ='To record adjustments to interest expense' or $varSubAccountDesc ='To record interest expense'">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record adjustment on Swap Accrual'">
                  <xsl:value-of select="'SwapInterestReceivable'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record admin fee payable'">
                  <xsl:value-of select="'AdminExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record amortization of organizational cost'">
                  <xsl:value-of select="'ORG EXP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record Clearing Fee expense charged by broker.' or $varSubAccountDesc ='To record payment of Clearing fees'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record dividend income'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record financial statements preparation fees'">
                  <xsl:value-of select="'Statements Preparation fees'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record Interest - Swaps'">
                  <xsl:value-of select="'SwapInterestExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record management fee'">
                  <xsl:value-of select="'MGMTFEE'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record other expense due to cash difference because of rounding off difference.'">
                  <xsl:value-of select="'OtherExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record receipt of Dividend Receivable'">
                  <xsl:value-of select="'Dividend_Receivable'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record reclaimable tax'">
                  <xsl:value-of select="'TAX_EXP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record research fee'">
                  <xsl:value-of select="'ResearchFeesPayable'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record withholding tax expense on Dividend income from Phineus Master'">
                  <xsl:value-of select="'WT'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Transfer' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Transfer' and $Cash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Deposit' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Deposit' and $Cash &lt; 0">
                  <xsl:value-of select="'CASH_WDL'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Research'">
                  <xsl:value-of select="'ResearchExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Accounting'">
                  <xsl:value-of select="'ACCTG'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Admin'">
                  <xsl:value-of select="'AdminExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Cash Adjustment' and $Cash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Cash Adjustment' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Payment of Admin Fees and other fees' and $Cash &lt; 0">
                  <xsl:value-of select="'AdminExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Record Management Fee' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To adjust realized gain (loss) for rounding off differences with broker' and $Cash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To adjust realized gain (loss) for rounding off differences with broker' and $Cash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record administrative fees' and $Cash &lt; 0">
                  <xsl:value-of select="'AdminExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record ADR Fee Expense' and $Cash &lt; 0">
                  <xsl:value-of select="'ADR Fee'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record amortization of organizational cost' or $varSubAccountDesc ='To record amortization of prepaid expenses') and $Cash &lt; 0">
                  <xsl:value-of select="'AMORT'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record audit fees' and $Cash &lt; 0">
                  <xsl:value-of select="'AUDITEXP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record payment of consulting fees' and $Cash &lt; 0">
                  <xsl:value-of select="'ConsultingExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record payment of legal fees' and $Cash &lt; 0">
                  <xsl:value-of select="'LEGALEXP'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record payment of various expenses' or $varSubAccountDesc ='To record software fees') and $Cash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record research expense' or $varSubAccountDesc ='To record research expense payments') and $Cash &lt; 0">
                  <xsl:value-of select="'ResearchExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record Short Rebate Expense' and $Cash &lt; 0">
                  <xsl:value-of select="'REB EXP'"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME_POST">
              <xsl:choose>

                <xsl:when test="$varSubAccountDesc ='AccountingRelated' or $varSubAccountDesc ='Deposit'
                          or $varSubAccountDesc ='GrossAmountInterest'
                          or $varSubAccountDesc ='To record amortization of organizational cost' or $varSubAccountDesc ='To record financial Statements Preparation fees' 
                          or $varSubAccountDesc ='To record Interest - Swaps' or $varSubAccountDesc ='To record interest expense' or $varSubAccountDesc ='To record management fee'
                          or $varSubAccountDesc ='To record other expense due to cash difference because of rounding off difference.' 
                          or $varSubAccountDesc ='To record payment of Clearing fees' or $varSubAccountDesc ='To record reclaimable tax' or $varSubAccountDesc ='To record research fee'
                          or $varSubAccountDesc ='To record withholding tax expense on Dividend income from Phineus Master'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record adjustment on Swap Accrual'">
                  <xsl:value-of select="'SwapInterestExpense'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record adjustments to interest expense'">
                  <xsl:value-of select="'ClearingExpenses'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record admin fee payable'">
                  <xsl:value-of select="'Audit'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record Clearing Fee expense charged by broker.'">
                  <xsl:value-of select="'CLEAR FEE'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record dividend income' or $varSubAccountDesc ='To record receipt of Dividend Receivable'">
                  <xsl:value-of select="'Dividend_Income'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Transfer' and $Cash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Transfer' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Deposit' and $Cash &gt; 0">
                  <xsl:value-of select="'CASH_DEP'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Deposit' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Research'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Accounting'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Admin'">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Cash Adjustment' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Cash Adjustment' and $Cash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Interest' and $Cash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Payment of Admin Fees and other fees' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='Record Management Fee' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To adjust realized gain (loss) for rounding off differences with broker' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To adjust realized gain (loss) for rounding off differences with broker' and $Cash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record administrative fees' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record ADR Fee Expense' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record amortization of organizational cost' or $varSubAccountDesc ='To record amortization of prepaid expenses') and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record audit fees' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record payment of consulting fees' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record payment of legal fees' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record payment of various expenses' or $varSubAccountDesc ='To record software fees') and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="($varSubAccountDesc ='To record research expense' or $varSubAccountDesc ='To record research expense payments') and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:when test="$varSubAccountDesc ='To record Short Rebate Expense' and $Cash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL8"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varCurrency" select="normalize-space(COL11)"/>
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>

            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$Cash &gt; 0">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$Cash &lt; 0">
                  <xsl:value-of select="$Cash*-1"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|' , $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
            </JournalEntries>

            <xsl:variable name="varDay">
              <xsl:choose>
                <xsl:when test="contains(COL3,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL3),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL3,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL3,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL3,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL3),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL3,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL3,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varMonth">
              <xsl:choose>
                <xsl:when test="contains(COL3,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL3),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL3,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL3,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL3,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL3),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL3,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL3,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:choose>
                <xsl:when test="contains(COL3,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL3,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL3,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL3,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <Date>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </Date>

            <xsl:variable name="Description" select="normalize-space(COL5)"/>
            <Description>
              <xsl:value-of select="$Description"/>
            </Description>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>