<?xml version="1.0" encoding="UTF-8"?>
<!--
Client: Saratoga 
Prime Broker: JPM
Description: one static row is added for column headers(requirement was like we have to add while spaces in the column headers
             so a static row added in the begining and write in the file and looks like a column header)
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>
        <!-- for system use only-->
        <FileHeader>
          <xsl:value-of select ="'true'"/>
        </FileHeader>
        <!-- for system use only-->
        <FileFooter>
          <xsl:value-of select ="'true'"/>
        </FileFooter>
        <!-- for system use only-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>
        <!-- for system use only-->
        <TaxlotState>
          <xsl:value-of select ="'Allocated'"/>
        </TaxlotState>

        <TRADETYPE >
          <xsl:value-of select="'TRADE TYPE'"/>
        </TRADETYPE>

        <TRADINGACCT>
          <xsl:value-of select="'TRADING ACCT'"/>
        </TRADINGACCT>

        <TRADEDATE>
          <xsl:value-of select="'TRADE DATE'"/>
        </TRADEDATE>

        <SETTLEDATE>
          <xsl:value-of select="'SETTLE DATE'"/>
        </SETTLEDATE>

        <BUY_SELL_IND>
          <xsl:value-of select="'BUY/SELL IND'"/>
        </BUY_SELL_IND>

        <QUANTITY>
          <xsl:value-of select ="'QUANTITY'"/>
        </QUANTITY>

        <SYMBOL>
          <xsl:value-of select="'SYMBOL'"/>
        </SYMBOL>

        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <COMMCODE>
          <xsl:value-of select="'COMM CODE'"/>
        </COMMCODE>

        <COMM>
          <xsl:value-of select ="'COMM $'"/>
        </COMM>

        <RR>
          <xsl:value-of select ="'RR'"/>
        </RR>

        <EXECBRKR>
          <xsl:value-of select="'EXEC BRKR'"/>
        </EXECBRKR>

        <CONTRABRKER>
          <xsl:value-of select ="'CONTRA BRKER'"/>
        </CONTRABRKER>

        <TAG>
          <xsl:value-of select ="'TAG'"/>
        </TAG>

        <BLOTTER>
          <xsl:value-of select="'BLOTTER'"/>
        </BLOTTER>

        <GPF>
          <xsl:value-of select="'GPF'"/>
        </GPF>

        <PUT_CALL_IND>
          <xsl:value-of select ="'PUT/CALL IND'"/>
        </PUT_CALL_IND>

        <STRIKEPX>
          <xsl:value-of select ="'STRIKE PX'"/>
        </STRIKEPX>

        <EXPIRATIONDATE>
          <xsl:value-of select="'EXPIRATION DATE'"/>
        </EXPIRATIONDATE>

        <CLIENTID>
          <xsl:value-of select ="'CLIENT ID'"/>
        </CLIENTID>

        <MTCHDCXL>
          <xsl:value-of select ="'MTCHD CXL'"/>
        </MTCHDCXL>

        <WI>
          <xsl:value-of select ="'WI'"/>
        </WI>

        <TCODE1>
          <xsl:value-of select="'TCODE 1'"/>
        </TCODE1>

        <TCODE2>
          <xsl:value-of select="'TCODE 2'"/>
        </TCODE2>

        <TCODE3>
          <xsl:value-of select="'TCODE 3'"/>
        </TCODE3>

        <TCODE4>
          <xsl:value-of select="'TCODE 4'"/>
        </TCODE4>

        <TCODE5>
          <xsl:value-of select="'TCODE 5'"/>
        </TCODE5>

        <TRADE_DESCRIPT_1>
          <xsl:value-of select="'TRADE DESCRIPT 1'"/>
        </TRADE_DESCRIPT_1>

        <TRADE_DESCRIPT_2>
          <xsl:value-of select="'TRADE DESCRIPT 2'"/>
        </TRADE_DESCRIPT_2>

        <TRADE_DESCRIPT_3>
          <xsl:value-of select="'TRADE DESCRIPT 3'"/>
        </TRADE_DESCRIPT_3>

        <TRADE_DESCRIPT_4>
          <xsl:value-of select="'TRADE DESCRIPT 4'"/>
        </TRADE_DESCRIPT_4>

        <TRADE_DESCRIPT_5>
          <xsl:value-of select="'TRADE DESCRIPT 5'"/>
        </TRADE_DESCRIPT_5>

        <TRADE_DESCRIPT_6>
          <xsl:value-of select="'TRADE DESCRIPT 6'"/>
        </TRADE_DESCRIPT_6>

        <TRADE_DESCRIPT_7>
          <xsl:value-of select="'TRADE DESCRIPT 7'"/>
        </TRADE_DESCRIPT_7>

        <TRADE_DESCRIPT_8>
          <xsl:value-of select="'TRADE DESCRIPT 8'"/>
        </TRADE_DESCRIPT_8>

        <!--for system internal use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>
					
          <!-- for system use only-->
          <FileHeader>
            <xsl:value-of select ="'true'"/>
          </FileHeader>
					
          <!-- for system use only-->
          <FileFooter>
            <xsl:value-of select ="'true'"/>
          </FileFooter>
          <!-- for system use only-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <!-- for system use only-->
          <TaxlotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxlotState>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption' or (Asset='Equity' and CounterParty='ENG')">
              <TRADETYPE>
                <xsl:value-of select="'BTS'"/>
              </TRADETYPE>
            </xsl:when>
            <xsl:otherwise>
              <TRADETYPE>
                <xsl:value-of select="'PRIME'"/>
              </TRADETYPE>
            </xsl:otherwise>
          </xsl:choose>

          <!--<TRADETYPE >
            <xsl:value-of select="'PRIME'" />
          </TRADETYPE>-->

          <TRADINGACCT>
            <xsl:value-of select="FundAccountNo"/>
          </TRADINGACCT>

          <TRADEDATE>
            <xsl:value-of select="TradeDate"/>
          </TRADEDATE>

          <SETTLEDATE>
            <xsl:value-of select="SettlementDate"/>
          </SETTLEDATE>

          <!-- Side Starts-->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open'">
              <BUY_SELL_IND>
                <xsl:value-of select="'B'"/>
              </BUY_SELL_IND>
            </xsl:when>
            <xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
              <BUY_SELL_IND>
                <xsl:value-of select="'BC'"/>
              </BUY_SELL_IND>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close'">
              <BUY_SELL_IND>
                <xsl:value-of select="'S'"/>
              </BUY_SELL_IND>
            </xsl:when>
            <xsl:when test="Side='Sell short' or Side='Sell to Open'">
              <BUY_SELL_IND>
                <xsl:value-of select="'SS'"/>
              </BUY_SELL_IND>
            </xsl:when>
            <xsl:otherwise >
              <BUY_SELL_IND>
                <xsl:value-of select="''"/>
              </BUY_SELL_IND>
            </xsl:otherwise>
          </xsl:choose >

          <QUANTITY>
            <xsl:value-of select ="AllocatedQty"/>
          </QUANTITY>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <SYMBOL>
                <xsl:value-of select="substring-before(Symbol,' ')"/>
              </SYMBOL>
            </xsl:when>
            <xsl:otherwise>
              <SYMBOL>
                <xsl:value-of select="Symbol"/>
              </SYMBOL>
            </xsl:otherwise>
          </xsl:choose>

          <PRICE>
            <xsl:value-of select="AveragePrice"/>
          </PRICE>

          <COMMCODE>
            <xsl:value-of select="'F'"/>
          </COMMCODE>

          <COMM>
            <xsl:value-of select ="CommissionCharged + TaxOnCommissions + OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
          </COMM>

          <RR>
            <xsl:value-of select ="''"/>
          </RR>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption' or (Asset='Equity' and CounterParty='ENG')">
              <EXECBRKR>
                <xsl:value-of select="'BEST'"/>
              </EXECBRKR>
            </xsl:when>
            <xsl:otherwise>
              <EXECBRKR>
                <xsl:value-of select="CounterParty"/>
              </EXECBRKR>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption' or (Asset='Equity' and CounterParty='ENG')">
              <CONTRABRKER>
                <xsl:value-of select="'BEST'"/>
              </CONTRABRKER>
            </xsl:when>
            <xsl:otherwise>
              <CONTRABRKER>
                <xsl:value-of select ="''"/>
              </CONTRABRKER>
            </xsl:otherwise>
          </xsl:choose>

          <TAG>
            <xsl:value-of select ="''"/>
          </TAG>


          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <BLOTTER>
                <xsl:value-of select="'3E'"/>
              </BLOTTER>
            </xsl:when>
						<xsl:when test ="Asset = 'Equity' and Exchange = 'NYSE'">
							<BLOTTER>
								<xsl:value-of select="'18'"/>
							</BLOTTER>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity' and Exchange = 'AMEX'">
							<BLOTTER>
								<xsl:value-of select="'28'"/>
							</BLOTTER>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity' and CounterParty !='ENG'">
							<BLOTTER>
								<xsl:value-of select="''"/>
							</BLOTTER>
						</xsl:when>
						<xsl:otherwise>
							<BLOTTER>
								<xsl:value-of select="'68'"/>
							</BLOTTER>
						</xsl:otherwise>
					</xsl:choose>
					
					<!--<xsl:choose>
						<xsl:when test ="Asset='EquityOption'">
							<BLOTTER>
								<xsl:value-of select="'3E'"/>
							</BLOTTER>
						</xsl:when>
            <xsl:when test ="Asset != 'EquityOption' and Exchange = 'NYSE'">
              <BLOTTER>
                <xsl:value-of select="'18'"/>
              </BLOTTER>
            </xsl:when>
            <xsl:when test ="Asset != 'EquityOption' and Exchange = 'AMEX'">
              <BLOTTER>
                <xsl:value-of select="'28'"/>
              </BLOTTER>
            </xsl:when>
            <xsl:otherwise>
              <BLOTTER>
                <xsl:value-of select="'68'"/>
              </BLOTTER>
            </xsl:otherwise>
					</xsl:choose>-->

          <GPF>
            <xsl:value-of select ="''"/>
          </GPF>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <PUT_CALL_IND>
                <xsl:value-of select ="substring(PutOrCall,1,1)"/>
              </PUT_CALL_IND>
            </xsl:when>
            <xsl:otherwise>
              <PUT_CALL_IND>
                <xsl:value-of select ="''"/>
              </PUT_CALL_IND>
            </xsl:otherwise>
          </xsl:choose>


					<xsl:choose>
						<xsl:when test ="Asset='EquityOption'">
							<STRIKEPX>
								<xsl:value-of select ="StrikePrice"/>
							</STRIKEPX>
						</xsl:when>
						<xsl:otherwise>
          <STRIKEPX>
            <xsl:value-of select ="''"/>
          </STRIKEPX>
						</xsl:otherwise>
					</xsl:choose>
					
					<!--<STRIKEPX>
						<xsl:value-of select ="StrikePrice"/>
					</STRIKEPX>-->

          <xsl:variable name = "varExpMth" >
            <xsl:value-of select="substring(ExpirationDate,1,2)"/>
          </xsl:variable>
          <xsl:variable name = "varExpYR" >
            <xsl:value-of select="substring(ExpirationDate,9,2)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <EXPIRATIONDATE>
                <!--<xsl:value-of select="concat($varExpMth,'/',$varExpYR)"/>-->
                <xsl:value-of select="ExpirationDate"/>
              </EXPIRATIONDATE>
            </xsl:when>
            <xsl:otherwise>
              <EXPIRATIONDATE>
                <xsl:value-of select ="''"/>
              </EXPIRATIONDATE>
            </xsl:otherwise>
          </xsl:choose>

          <CLIENTID>
            <xsl:value-of select="TradeRefID"/>
          </CLIENTID>

          <MTCHDCXL>
            <xsl:value-of select ="''"/>
          </MTCHDCXL>

          <WI>
            <xsl:value-of select ="''"/>
          </WI>

          <TCODE1>
            <xsl:value-of select="''"/>
          </TCODE1>

          <TCODE2>
            <xsl:value-of select="''"/>
          </TCODE2>

          <TCODE3>
            <xsl:value-of select="''"/>
          </TCODE3>

          <TCODE4>
            <xsl:value-of select="''"/>
          </TCODE4>

          <TCODE5>
            <xsl:value-of select="''"/>
          </TCODE5>

          <TRADE_DESCRIPT_1>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_1>

          <TRADE_DESCRIPT_2>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_2>

          <TRADE_DESCRIPT_3>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_3>

          <TRADE_DESCRIPT_4>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_4>

          <TRADE_DESCRIPT_5>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_5>

          <TRADE_DESCRIPT_6>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_6>

          <TRADE_DESCRIPT_7>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_7>

          <TRADE_DESCRIPT_8>
            <xsl:value-of select="''"/>
          </TRADE_DESCRIPT_8>

          <!--for system internal use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
