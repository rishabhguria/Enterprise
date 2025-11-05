<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <!--<ThirdPartyFlatFileDetail>
        --><!--for system internal use--><!--
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        --><!--for system use only--><!--
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        --><!--for system internal use--><!--
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <Item>
          <xsl:value-of select="'Item'"/>
        </Item>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Security>
          <xsl:value-of select="'Security'"/>
        </Security>

        <Description>
          <xsl:value-of select="'Description'"/>
        </Description>

        <CUSIP>
          <xsl:value-of select="'Cusip'"/>
        </CUSIP>

        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <Amount>
          <xsl:value-of select="'Amount'"/>
        </Amount>

        <AvgPrice>
          <xsl:value-of select="'Avg. Price'"/>
        </AvgPrice>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <BrokerLong>
          <xsl:value-of select="'BrokerLong'"/>
        </BrokerLong>

        <DTC>
          <xsl:value-of select="'DTC'"/>
        </DTC>

        <EBic>
          <xsl:value-of select="'eBIC'"/>
        </EBic>
        
        <CBic>
          <xsl:value-of select="'cBIC'"/>
        </CBic>

        <TotalComm>
          <xsl:value-of select="'TotalCommission'"/>
        </TotalComm>

        <PrincipalAmount>
          <xsl:value-of select="'Principal Amount'"/>
        </PrincipalAmount>

        <NetAmount>
          <xsl:value-of select="'Net Amount'"/>
        </NetAmount>
		 
        <Country>
          <xsl:value-of select="'Country'"/>
        </Country>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <Interest>
          <xsl:value-of select="'Interest'"/>
        </Interest>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <Underlying>
          <xsl:value-of select="'Underlying'"/>
        </Underlying>

        <TYPE>
          <xsl:value-of select="'TYPE'"/>
        </TYPE>

        <Strike>
          <xsl:value-of select="'Strike'"/>
        </Strike>

        <Expiration>
          <xsl:value-of select="'Expiration'"/>
        </Expiration>

        <AccountNumber>
          <xsl:value-of select="'AccountNumber'"/>
        </AccountNumber>

        <Fees>
          <xsl:value-of select="'Fees'"/>
        </Fees>
        
        --><!-- system use only--><!--
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>-->

      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'naveen1' and Asset != 'FXForward']">
		  <!--<xsl:if test ="(AccountName = 'Beachhead-Poco Bay-61160578') and Asset != 'FXForward'">-->

			  <ThirdPartyFlatFileDetail>
				  <!--for system internal use-->
				  <RowHeader>
					  <xsl:value-of select ="true"/>
				  </RowHeader>

				  <!--for system use only-->
				  <!--<IsCaptionChangeRequired>
					  <xsl:value-of select ="true"/>
				  </IsCaptionChangeRequired>-->

				  <!--for system internal use-->
				  <TaxLotState>
					  <xsl:value-of select ="TaxLotState"/>
				  </TaxLotState>

				  <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
				  <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>


				  <xsl:variable name="varSide">
					  <xsl:choose>
						  <xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
							  <xsl:value-of select="'Short'"/>
						  </xsl:when>
						  
						  <xsl:otherwise>
							  <xsl:value-of select="Side"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <!--Exercise / Assign Need To Ask-->
				  <Item>
					  <xsl:value-of select="PBUniqueID"/>
				  </Item>

				  <Side>
					  <xsl:value-of select="$varSide"/>
				  </Side>

				  <Security>
					  <xsl:choose>
						  <xsl:when test="Asset = 'EquityOption'">
							  <xsl:value-of select="OSIOptionSymbol"/>
						  </xsl:when>
						  <xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity'">
							  <xsl:value-of select ="SEDOL"/>
						  </xsl:when>
						  <xsl:when test="contains(Symbol, '-') != false">
							  <xsl:value-of select="ISIN"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="Symbol"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Security>

				  <Description>
					  <xsl:value-of select="FullSecurityName"/>
				  </Description>

				  <CUSIP>
					  <xsl:value-of select="CUSIP"/>
				  </CUSIP>

				  <SEDOL>
					  <xsl:choose>
						  <xsl:when test="Asset = 'EquityOption'">
							  <xsl:value-of select="OSIOptionSymbol"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="SEDOL"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SEDOL>

				  <ISIN>
					  <xsl:value-of select="ISIN"/>
				  </ISIN>

				  <Amount>
					  <xsl:value-of select="AllocatedQty"/>
				  </Amount>

				  <AvgPrice>
					  <xsl:value-of select="AveragePrice"/>
				  </AvgPrice>

				  <Broker>
					  <xsl:choose>
						  <xsl:when test ="CounterParty = 'JEFFD'">
							  <xsl:value-of select ="'JEFF'"/>
						  </xsl:when>
						  <xsl:when test ="CounterParty = 'LAZAD'">
							  <xsl:value-of select ="'LAZA'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="CounterParty"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Broker>

				  <BrokerLong>
					  <xsl:choose>
						  <xsl:when test ="CounterParty = 'JEFFD'">
							  <xsl:value-of select ="'JEFF'"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="CounterParty"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </BrokerLong>

				  <DTC>
					  <xsl:value-of select="'19'"/>
				  </DTC>

				  <EBic>
					  <xsl:value-of select="substring(CurrencySymbol,1,2)"/>
				  </EBic>

				  <CBic>
					  <xsl:value-of select="substring(CurrencySymbol,1,2)"/>
				  </CBic>

				  <TotalComm>
					  <xsl:value-of select="CommissionCharged"/>
				  </TotalComm>

				  <PrincipalAmount>
					  <xsl:value-of select="GrossAmount"/>
				  </PrincipalAmount>

				  <NetAmount>					  
					  <xsl:value-of select="NetAmount"/>
				  </NetAmount>

				  <Country>
					  <xsl:value-of select="CurrencySymbol"/>
				  </Country>

				  <Currency>
					  <xsl:value-of select="CurrencySymbol"/>
				  </Currency>

				  <Interest>
					  <xsl:value-of select="AccruedInterest"/>
				  </Interest>

				  <TradeDate>
					  <xsl:value-of select="TradeDate"/>
				  </TradeDate>

				  <SettleDate>
					  <xsl:value-of select="SettlementDate"/>
				  </SettleDate>

				  <Underlying>
					  <xsl:choose>
						  <xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity'">
							  <xsl:value-of select ="SEDOL"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="UnderlyingSymbol"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Underlying>

				  <TYPE>
					  <xsl:value-of select="substring(PutOrCall,1,1)"/>
				  </TYPE>

				  <Strike>
					  <xsl:choose>
						  <xsl:when test="number(StrikePrice)">
							  <xsl:value-of select="StrikePrice"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Strike>

				  <Expiration>
					  <xsl:choose>
						  <xsl:when test="ExpirationDate = '01/01/1800'">
							  <xsl:value-of select="''"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="ExpirationDate"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Expiration>

				  <AccountNumber>
					  <xsl:value-of select="'61160578'"/>
				  </AccountNumber>

				  <Fees>
					  <xsl:value-of select="format-number(StampDuty+TransactionLevy,'#.00')"/>
				  </Fees>

				  <!-- system use only-->
				  <EntityID>
					  <xsl:value-of select="EntityID"/>
				  </EntityID>

			  </ThirdPartyFlatFileDetail>
		  <!--</xsl:if>-->
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
