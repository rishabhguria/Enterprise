<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Symbol!='PISXX' and Symbol!='DREUSTB']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxlotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxlotState>

			<xsl:variable name="PB_NAME" select="'US Bancorp'"/>
			<xsl:variable name = "PRANA_FUND_NAME">
				<xsl:value-of select="FundName"/>
			</xsl:variable>

			<xsl:variable name ="THIRDPARTY_FUND_CODE">
				<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
			</xsl:variable>

			<xsl:variable name="FundCode">
				<xsl:value-of select="FundAccountNo"/>
				<!--<xsl:choose>
					<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
						<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PRANA_FUND_NAME"/>
					</xsl:otherwise>
				</xsl:choose>-->
			</xsl:variable>

			<xsl:variable name="TransactionType">
				<xsl:choose>
					<xsl:when test="TaxLotState='Allocated'">
						<xsl:value-of select="'NEW'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Deleted'">
						<xsl:value-of select="'CAN'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Amended'">
						<xsl:value-of select="'COR'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="Year" select="substring-after(substring-after(TradeDate,'/'),'/')"/>
			<xsl:variable name="Month" select="substring-before(substring-after(TradeDate,'/'),'/')"/>
			<xsl:variable name="Day" select="substring-before(TradeDate,'/')"/>

          <TradeID>
			  <!--<xsl:choose>
				  <xsl:when test="contains(FundName,'QMF-Wells')">
					  <xsl:value-of select="'2015_12_3_CMG CN_ABLQF_New'"/>
				  </xsl:when>
				  
			  </xsl:choose>-->
            <xsl:value-of select="concat($Year,'_',$Month,'_',$Day,'_',Symbol,'_',EntityID)"/>
          </TradeID>


         


          <FundCode>
			  <xsl:value-of select="$FundCode"/>
          </FundCode>



          <TransactionType>

			  <xsl:value-of select="$TransactionType"/>

          </TransactionType>

          <ProductType>
            <xsl:choose>
              <xsl:when test="contains(Asset,'FX')">
                <xsl:value-of select="'Currency'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>


          </ProductType>

          <BloombergYellowKey>
            <xsl:value-of select="''"/>

          </BloombergYellowKey>
          


          <Ticker>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="OSIOptionSymbol"/> 
				  </xsl:when>
				  <xsl:when test="Asset='Future'">
					  <xsl:value-of select="BBCode"/>
				  </xsl:when>
				  <xsl:when test="contains(BBCode,'EQUITY') or contains(BBCode,'CURNCY')">
					  <xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
				  </xsl:when>
				 
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
            

          </Ticker>

          <ISIN>
            <xsl:value-of select="ISIN"/>


          </ISIN>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>

          </CUSIP>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>

          </SEDOL>

          <OCCCode>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>


          </OCCCode>

          <ISOCurrencyCode>
            <xsl:choose>
              <xsl:when test="contains(Asset,'FX')">
                <xsl:value-of select="LeadCurrencyName"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:otherwise>
            </xsl:choose>


          </ISOCurrencyCode>

          <PrivateAssetID>

            <xsl:value-of select="''"/>
          </PrivateAssetID>

          <TradeType>
            <xsl:value-of select="Side"/>

          </TradeType>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>

          </Quantity>

          <Price>
			  <xsl:choose>
				  <xsl:when test="contains(Asset,'FX')">
					  <xsl:value-of select="format-number(AveragePrice,'0.########')"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="AveragePrice"/>
				  </xsl:otherwise>
			  </xsl:choose>


		  </Price>

          <Factor>
            <xsl:value-of select="''"/>
          </Factor>

          <LocalInterest>

            <xsl:value-of select="''"/>

          </LocalInterest>

          <NetProceeds>
            <xsl:value-of select="NetAmount"/>

          </NetProceeds>

          <Currency>

			  <xsl:choose>
				  <xsl:when test="contains(Asset,'FX')">
					  <xsl:value-of select="VsCurrencyName"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="CurrencySymbol"/>
				  </xsl:otherwise>
			  </xsl:choose>

          </Currency>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>

          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>

          </SettleDate>




          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterPartyID"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
          </xsl:variable>

          <xsl:variable name="Broker">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ExecutingBroker>
            <xsl:value-of select="$Broker"/>
          </ExecutingBroker>



          <ClearingBroker_Custodian_FCM>
			  <xsl:choose>
				  <xsl:when test="Asset = 'Equity'">
					  <xsl:value-of select="'SS.NYC.PB'"/>
				  </xsl:when>
				  <xsl:when test="Asset = 'Future'">
					  <xsl:value-of select="'SS.NYC.FUTURE'"/>
				  </xsl:when>
			  </xsl:choose>
           
          </ClearingBroker_Custodian_FCM>

          <Book>

            <xsl:value-of select="''"/>
          </Book>

          <Strategy>
            <xsl:value-of select="''"/>


          </Strategy>

          <Exchange>
            <xsl:value-of select="''"/>

          </Exchange>

          <UnderlyingAssetType>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption' or Asset='Future'">
                <xsl:value-of select="Asset"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>

          </UnderlyingAssetType>

          <UnderlyingID>
            <xsl:value-of select="''"/>

          </UnderlyingID>

          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + MiscFees +  OtherBrokerFee,'0.##')"/>

          </Commission>

			<OrfFee>
				<xsl:choose>
					<xsl:when test="Asset='EquityOption'">
						<xsl:value-of select="OrfFee"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
				
			</OrfFee>

			
			<DTCCode>
				<xsl:choose>
					<xsl:when test="CounterParty='MSCO'">
						<xsl:value-of select="'050'"/>
					</xsl:when>
					<xsl:when test="CounterParty='MSCO'">
						<xsl:value-of select="'050'"/>
					</xsl:when>
					<xsl:when test="CounterParty='BTIG'">
						<xsl:value-of select="'005'"/>
					</xsl:when>
					<xsl:when test="CounterParty='TDMG'">
						<xsl:value-of select="'443'"/>
					</xsl:when>
					<xsl:when test="CounterParty='GSCO'">
						<xsl:value-of select="'005'"/>
					</xsl:when>
					<xsl:when test="CounterParty='IMPC'">
						<xsl:value-of select="'443'"/>
					</xsl:when>
					<xsl:when test="CounterParty='BARC'">
						<xsl:value-of select="'229'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</DTCCode>



					<EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>