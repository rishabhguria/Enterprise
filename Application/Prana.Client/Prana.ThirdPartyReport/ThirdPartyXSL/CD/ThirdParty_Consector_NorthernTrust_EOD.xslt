<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
  
       <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'false'"/>
          </IsCaptionChangeRequired>

          <FileHeader>
            <xsl:value-of select ="'false'"/>
          </FileHeader>
          <FileFooter>
            <xsl:value-of select ="'false'"/>
          </FileFooter>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <EXTERNALREFERENCEID>
            <xsl:value-of select="EntityID"/>
          </EXTERNALREFERENCEID>

          <SIDE>
			  <xsl:value-of select ="substring(Side,1,1)"/>
            <!--<xsl:choose>
              <xsl:when test="Side = 'Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Open'">
                <xsl:value-of select="'BO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Open'">
                <xsl:value-of select="'SO'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
            </xsl:choose>-->
          </SIDE>

          <FINANCIALTYPE>
			  <xsl:choose>
				  <xsl:when test ="Asset = 'EquityOption'">
					  <xsl:value-of select ="'OPTION'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'COMMON'"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </FINANCIALTYPE>

          <TRADEDATE>
            <xsl:value-of select="TradeDate"/>
          </TRADEDATE>

          <CLEARINGCOUNTERPARTY>
            <xsl:value-of select="''"/>
          </CLEARINGCOUNTERPARTY>

          <EXECUTINGCOUNTERPARTY>
            <xsl:value-of select="CounterParty"/>
          </EXECUTINGCOUNTERPARTY>

          <INSTRUMENTIDENTIFIERTYPE>
			  <xsl:choose>
				  <xsl:when test ="Asset = 'EquityOption'">
					  <xsl:value-of select ="'OCC SYMBOL'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'CUSIP'"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </INSTRUMENTIDENTIFIERTYPE>

          <INSTRUMENTIDENTIFIER>
			  <xsl:choose>
				  <xsl:when test ="Asset = 'EquityOption'">
					  <xsl:value-of select ="OSIOptionSymbol"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="CUSIP"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </INSTRUMENTIDENTIFIER>

          <PBR>
            <xsl:value-of select="'TRADING'"/>
          </PBR>


          <CASHSUBACCOUNT>
            <xsl:choose>
				<xsl:when test ="Asset = 'EquityOption'">
					<xsl:value-of select ="'MARGIN'"/>
				</xsl:when>
              <xsl:when  test="Side = 'Buy' or Side = 'Sell' or Side = 'Buy to Open' or Side = 'Sell to Close'">
                <xsl:value-of select="'MARGIN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'SHORT'"/>
              </xsl:otherwise>
            </xsl:choose>
          </CASHSUBACCOUNT>

          <FUND>
            <xsl:value-of select="'CMLP'"/>
          </FUND>

          <CURRENCY>
            <xsl:value-of select="CurrencySymbol"/>
          </CURRENCY>

          <QUANTITY>
            <xsl:value-of select="AllocatedQty"/>
          </QUANTITY>

          <PRICE>
            <xsl:value-of select="format-number(AveragePrice, '#.0000')"/>
          </PRICE>

          <SETTLEDATE>
            <xsl:value-of select="SettlementDate"/>
          </SETTLEDATE>

          <NOTES>
            <xsl:value-of select="''"/>
          </NOTES>

          <xsl:variable name="varFundName">
            <xsl:value-of select="translate(AccountName , $vLowercaseChars_CONST, $vUppercaseChars_CONST)"/>
          </xsl:variable>

          <CLEARINGACCOUNT>
            <xsl:choose>
              <xsl:when test="$varFundName = 'CONSECTOR PARTNERS MASTER FUND LP:75203752'">
                <xsl:value-of select="'UBSW-DPB-CMLP'"/>
              </xsl:when>
				<xsl:when test="$varFundName = 'CONSECTOR PARTNERS LP:75200926'">
					<xsl:value-of select="'UBS3-DPB-CMLP'"/>
				</xsl:when>
				<xsl:when test="$varFundName = 'CONSECTOR PARTNERS MASTER FUND LP:75203753'">
					<xsl:value-of select="'UBS2-DPB-CMLP'"/>
				</xsl:when>
				<xsl:when test="$varFundName = '9583729 - CONSECTOR PARTNERS MF LP-ESC - 75203753'">
					<xsl:value-of select="'UBS2-DPB-CMLP'"/>
				</xsl:when>
              <xsl:when test="$varFundName = '3491823 - CONSECTOR PARTNERS LP ESC - 75200927'">
                <xsl:value-of select="'UBS2-DPB-CMLP'"/>
              </xsl:when>

				<xsl:when test="$varFundName = 'HOLD-BOX-CMLP '">
					<xsl:value-of select="'HOLD-BOX-CMLP'"/>
				</xsl:when>
				
				
				
            </xsl:choose>
          </CLEARINGACCOUNT>

			<POSITIONTYPE>
				<xsl:choose>
					<xsl:when test ="Asset = 'EquityOption' and contains(Side, 'Open') != false">
						<xsl:value-of select ="'0'"/>
					</xsl:when>
					<xsl:when test ="Asset = 'EquityOption' and contains(Side, 'Close') != false">
						<xsl:value-of select ="'1'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</POSITIONTYPE>

          <COMMISSIONRATE>
            <xsl:value-of select="CommissionCharged"/>
          </COMMISSIONRATE>
          
          <COMMISSIONTYPE>
            <xsl:value-of select="'EXPLICIT'"/>
          </COMMISSIONTYPE>

          <TRADERID>
            <xsl:value-of select="'CONSECTOR'"/>
          </TRADERID>

          <STRATEGYID>
            <xsl:value-of select="''"/>
          </STRATEGYID>

          <DESK>
            <xsl:value-of select="'CCAP'"/>
          </DESK>

			<EXPIRYDATE>
				<xsl:choose>
					<xsl:when test ="Asset = 'EquityOption' ">
						<xsl:value-of select ="ExpirationDate"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</EXPIRYDATE>

			<OPTIONTYPE>
				<xsl:choose>
					<xsl:when test ="Asset = 'EquityOption' ">
						<xsl:value-of select ="PutOrCall"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</OPTIONTYPE>

			<STRIKEPRICE >
				<xsl:choose>
					<xsl:when test ="Asset = 'EquityOption' ">
						<xsl:value-of select ="StrikePrice"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</STRIKEPRICE>

          <FEE_OTHER_FEE>
            <xsl:value-of select="''"/>
          </FEE_OTHER_FEE>

          <MSGTYPE>
            <xsl:value-of select="''"/>
          </MSGTYPE>

          <VALUATIONCURRENCY>
            <xsl:value-of select="''"/>
          </VALUATIONCURRENCY>
          
          
          <!--<STRATEGY>
            <xsl:value-of select="'TRADING'"/>
          </STRATEGY>

          <ISTRS>
            <xsl:value-of select="'Y'"/>
          </ISTRS>

          <DAYCOUNTMETHOD>
            <xsl:value-of select="''"/>
          </DAYCOUNTMETHOD>

          <DIVIDEND>
            <xsl:value-of select="''"/>
          </DIVIDEND>

          <INDEPENDENTPERCENT>
            <xsl:value-of select="''"/>
          </INDEPENDENTPERCENT>

          <INTEREST_INDEX>
            <xsl:value-of select="''"/>
          </INTEREST_INDEX>

          <SPREAD>
            <xsl:value-of select="''"/>
          </SPREAD>

          <SWAPTYPE>
            <xsl:value-of select="''"/>
          </SWAPTYPE>

          <TERMDATE>
            <xsl:value-of select="''"/>
          </TERMDATE>-->

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
