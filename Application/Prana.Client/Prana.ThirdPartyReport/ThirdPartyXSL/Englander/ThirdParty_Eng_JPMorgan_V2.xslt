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

        <SYMBOL>
          <xsl:value-of select="'SYMBOL'"/>
        </SYMBOL>

        <QUANTITY>
          <xsl:value-of select ="'QUANTITY'"/>
        </QUANTITY>      			

				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

        <EXECBRKR>
          <xsl:value-of select="'EXEC BRKR'"/>
        </EXECBRKR>

        <COMMCODE>
          <xsl:value-of select="'COMM CODE'"/>
        </COMMCODE>
        
        <COMM>
          <xsl:value-of select ="'COMM $'"/>
        </COMM>
        
        <RR>
          <xsl:value-of select ="'RR'"/>
        </RR>

        <ACCRINT>
          <xsl:value-of select ="'ACCR INT'"/>
        </ACCRINT>

        <BLOTTER>
          <xsl:value-of select="'BLOTTER'"/>
        </BLOTTER>

        <CONTRABRKER>
          <xsl:value-of select ="'CONTRA BRKER'"/>
        </CONTRABRKER>

        <PBALLOCIND>
          <xsl:value-of select ="'PB ALLOC IND'"/>
        </PBALLOCIND>

        <STRIKEPX>
          <xsl:value-of select ="'STRIKE PX'"/>
        </STRIKEPX>

        <PUT_CALL_IND>
          <xsl:value-of select ="'PUT/CALL IND'"/>
        </PUT_CALL_IND>       
       
        <EXPIRATIONDATE>
          <xsl:value-of select="'EXPIRATION DATE'"/>
        </EXPIRATIONDATE>

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

					<TRADETYPE >
					<xsl:value-of select="'PRIME'" />
					</TRADETYPE>					

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

          <SYMBOL>
            <xsl:value-of select="Symbol"/>
          </SYMBOL>

          <QUANTITY>
            <xsl:value-of select ="AllocatedQty"/>
          </QUANTITY>

          <PRICE>
            <xsl:value-of select="AveragePrice"/>
          </PRICE>

          <EXECBRKR>
            <xsl:value-of select="CounterParty"/>
          </EXECBRKR>

          <COMMCODE>
            <xsl:value-of select="'F'"/>
          </COMMCODE>

          <COMM>
            <xsl:value-of select ="CommissionCharged + TaxOnCommissions + OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
          </COMM>

          <RR>
            <xsl:value-of select ="'099'"/>
          </RR>

          <ACCRINT>
            <xsl:value-of select ="''"/>
          </ACCRINT>

          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <BLOTTER>
                <xsl:value-of select="'3E'"/>
              </BLOTTER>
            </xsl:when>
            <xsl:otherwise>
              <BLOTTER>
                <xsl:value-of select="''"/>
              </BLOTTER>
            </xsl:otherwise>
          </xsl:choose>

         

          <CONTRABRKER>
            <xsl:value-of select ="''"/>
          </CONTRABRKER>

          <PBALLOCIND>
            <xsl:value-of select ="''"/>
          </PBALLOCIND>

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

          <xsl:variable name = "varExpMth" >
            <xsl:value-of select="substring(ExpirationDate,1,2)"/>
          </xsl:variable>
          <xsl:variable name = "varExpYR" >
            <xsl:value-of select="substring(ExpirationDate,9,2)"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test ="Asset='EquityOption'">
              <EXPIRATIONDATE>
                <xsl:value-of select="concat($varExpMth,'/',$varExpYR)"/>
              </EXPIRATIONDATE>
            </xsl:when>
            <xsl:otherwise>
              <EXPIRATIONDATE>
                <xsl:value-of select ="''"/>
              </EXPIRATIONDATE>
            </xsl:otherwise>
          </xsl:choose>

					<!--for system internal use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
