<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Amount>
          <xsl:value-of select="'Amount'"/>
        </Amount>

        <Security>
          <xsl:value-of select="'Security'"/>
        </Security>

        <Done>
          <xsl:value-of select="'Done'"/>
        </Done>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>


        <Comm>
          <xsl:value-of select="'Comm'"/>
        </Comm>

        <TNum1>
          <xsl:value-of select="'TNum1'"/>
        </TNum1>

        <Fee>
          <xsl:value-of select="'Fee'"/>
        </Fee>

        <Manager>
          <xsl:value-of select ="'Manager'"/>
        </Manager>

        <Trader>
          <xsl:value-of select ="'Trader'"/>
        </Trader>

        <Prt>
          <xsl:value-of select ="'Prt'"/>
        </Prt>

        <Cust>
          <xsl:value-of select ="'Cust'"/>
        </Cust>

        <To_Curr>
          <xsl:value-of select="'To_Curr'"/>
        </To_Curr>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <xsl:if test ="AccountName != 'Weisbrod Family-JPM'">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
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

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Side>
            <xsl:choose>
              <xsl:when test="TaxLotState = 'Allocated' or TaxLotState = 'Sent'" >
                <xsl:choose>
                  <xsl:when test="Side = 'Buy' or Side = 'Sell'">
                    <xsl:value-of select="Side"/>
                  </xsl:when>
                  <xsl:when test="Side = 'Sell short'">
                    <xsl:value-of select="'Short'"/>
                  </xsl:when>
					<xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption'">
						<xsl:value-of select="'Short'"/>
					</xsl:when>
                  <xsl:when test="Side = 'Buy to Close'">
                    <xsl:value-of select="'Cover'"/>
                  </xsl:when>
                  <xsl:when test="Side = 'Sell to Open' or Side = 'Sell to Close'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side = 'Buy to Open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </Side>

          <Amount>
            <xsl:value-of select="AllocatedQty"/>
          </Amount>

          <Security>
			  <xsl:choose>
				  <xsl:when test ="(Asset = 'Equity' and contains(Symbol,'-') != false)">
					  <xsl:choose>
						  <xsl:when test ="contains(BBCode,'EQUITY')!= false">
							  <xsl:value-of select ="substring-before(BBCode,' EQUITY')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="BBCode"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:when>
				  <xsl:when test =" Asset = 'EquityOption' and contains(BBCode,'EQUITY')!= false and contains(BBCode,'US')!= false">
					  <xsl:value-of select ="concat(substring-before(BBCode,' '), ' ', substring-before(substring-after(substring-after(BBCode,' '),' '),' EQUITY'))"/>
				  </xsl:when>
				  <xsl:when test =" Asset = 'EquityOption' and contains(BBCode,'US')!= false">
					  <xsl:value-of select ="concat(substring-before(BBCode,' '),  substring-after(BBCode,'US'))"/>
				  </xsl:when>
				  <xsl:when test =" Asset = 'EquityOption'">
					  <xsl:value-of select ="BBCode"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </Security>

          <Done>
            <xsl:value-of select="AllocatedQty"/>
          </Done>

          <Broker>
			  <xsl:choose>
				  <xsl:when test ="CounterParty = 'BTIGI'">
					  <xsl:value-of select ="'BTIG'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="CounterParty"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </Broker>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <!--<xsl:variable name = "PRANA_CounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PRANA_CommissionCode">
            <xsl:value-of select="document('../ReconMappingXml/CommissionMapping.xml')/CommissionMapping/PB[@Name='JPM']/CommissionData[@PranaBroker=$PRANA_CounterParty]/@PranaCommissionCode"/>
          </xsl:variable>-->

          <Comm>
			  <xsl:choose>
				  <xsl:when test ="CommissionCharged = 0">
					  <xsl:value-of select ="'none'"/>
				  </xsl:when>
				  <xsl:when test="Asset != 'EquityOption'">
					  <xsl:value-of select="concat(format-number((CommissionCharged div AllocatedQty),'#.00##'),'e')"/>
				  </xsl:when>
				  <xsl:when test="Asset = 'EquityOption' and CurrencySymbol != 'USD'">
					  <xsl:value-of select="concat(format-number((CommissionCharged div AllocatedQty), '#.##'), translate(CurrencySymbol, $varCapital, $varSmall))"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select ="concat('$',CommissionCharged div AllocatedQty)"/>
				  </xsl:otherwise>
			  </xsl:choose>
            <!--<xsl:choose>
              <xsl:when test="(CounterParty ='BMOC' or CounterParty ='CSTI' or CounterParty ='SBSH' or CounterParty ='COWN' or CounterParty ='FBPC' or CounterParty ='DBPC' or CounterParty ='INCA' or CounterParty ='ISIG' or CounterParty ='GSCO' or CounterParty ='JPMS' or CounterParty ='JSSF' or CounterParty ='JEFF' or CounterParty ='LAZA' or CounterParty ='LEER' or CounterParty ='MLCO' or CounterParty ='MLCOO' or CounterParty ='MSCO' or CounterParty ='RBCM' or CounterParty ='CNFR' or CounterParty ='BTIG' or CounterParty ='LEHM' or CounterParty ='AABA' or CounterParty ='JPMS' or CounterParty ='CSTI' or CounterParty ='FBPC' or CounterParty ='INCA') and AveragePrice &lt; 10">
                <xsl:value-of select="'.03e'"/>
              </xsl:when>
              <xsl:when test="(CounterParty ='BMOC' or CounterParty ='CSTI' or CounterParty ='SBSH' or CounterParty ='COWN' or CounterParty ='FBPC' or CounterParty ='DBPC' or CounterParty ='INCA' or CounterParty ='ISIG' or CounterParty ='GSCO' or CounterParty ='JPMS' or CounterParty ='JSSF' or CounterParty ='JEFF' or CounterParty ='LAZA' or CounterParty ='LEER' or CounterParty ='MLCO' or CounterParty ='MLCOO' or CounterParty ='MSCO' or CounterParty ='RBCM' or CounterParty ='CNFR' or CounterParty ='BTIG' or CounterParty ='LEHM' or CounterParty ='AABA' or CounterParty ='JPMS' or CounterParty ='CSTI' or CounterParty ='FBPC' or CounterParty ='INCA') and AveragePrice >= 10">
                <xsl:value-of select="'.04e'"/>
              </xsl:when>
              <xsl:when test="(CounterParty ='BARCO' or CounterParty ='COWNO' or CounterParty ='GSCOO' or CounterParty ='MSCOO' or CounterParty ='INCAO' or CounterParty ='JEEFO' or CounterParty ='RBCMO') and AveragePrice >= 1">
                <xsl:value-of select="'$3'"/>                
              </xsl:when>
              <xsl:when test="(CounterParty ='BARCO' or CounterParty ='COWNO' or CounterParty ='GSCOO' or CounterParty ='MSCOO' or CounterParty ='INCAO' or CounterParty ='JEEFO' or CounterParty ='RBCMO') and AveragePrice &lt; 1">
                <xsl:value-of select="'$1'"/>
              </xsl:when>
            </xsl:choose>-->
          </Comm>

          <TNum1>
            <xsl:value-of select="''"/>
          </TNum1>

          <Fee>
            <xsl:value-of select="''"/>
          </Fee>

          <Manager>
            <xsl:value-of select ="'IGU'"/>
          </Manager>

          <Trader>
            <xsl:value-of select ="'NSC'"/>
          </Trader>

          <Prt>
            <xsl:choose>
              <xsl:when test="AccountName = 'Iguana Healthcare-JPM'">
                <xsl:value-of select="'iguanamf'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'Whitney Capital-JPM' or AccountName = 'New Issues-JPM'">
                <xsl:value-of select="'whitney'"/>
              </xsl:when>
				<xsl:when test="AccountName = 'Weisbrod Family-JPM'">
					<xsl:value-of select="'Weisbrod'"/>
				</xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Prt>

          <Cust>
            <xsl:value-of select ="'JPMSX'"/>
          </Cust>

          <To_Curr>
            <xsl:value-of select="CurrencySymbol"/>
          </To_Curr>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
			  </xsl:if>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	
</xsl:stylesheet>
