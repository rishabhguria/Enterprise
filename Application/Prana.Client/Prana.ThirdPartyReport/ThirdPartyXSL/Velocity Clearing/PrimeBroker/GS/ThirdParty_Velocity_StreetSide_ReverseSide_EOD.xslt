<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <ACCOUNT>
          <xsl:value-of select="'ACCOUNT'"/>
        </ACCOUNT>

        <SIDE>
          <xsl:value-of select="'SIDE'"/>
        </SIDE>


        <QUANTITY>
          <xsl:value-of select="'QUANTITY'"/>
        </QUANTITY>


        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <COMMISSION>
          <xsl:value-of select="'COMMISSION'"/>
        </COMMISSION>

 <BrokerCode>
            <xsl:value-of select="'BrokerCode'"/>
          </BrokerCode>

        <TRADEDATEYEAR>
          <xsl:value-of select="'TRADEDATEYEAR'"/>
        </TRADEDATEYEAR>

        <TRADEDATEMONTH>
          <xsl:value-of select="'TRADEDATEMONTH'"/>
        </TRADEDATEMONTH>

        <TRADEDATEDAY>
          <xsl:value-of select="'TRADEDATEDAY'"/>
        </TRADEDATEDAY>

        <SETTLEDATEYEAR>
          <xsl:value-of select="'SETTLEDATEYEAR'"/>
        </SETTLEDATEYEAR>

        <SETTLEDATEMONTH>
          <xsl:value-of select="'SETTLEDATEMONTH'"/>
        </SETTLEDATEMONTH>

        <SETTLEDATEDAY>
          <xsl:value-of select="'SETTLEDATEDAY'"/>
        </SETTLEDATEDAY>

        <MEMOFIELD>
          <xsl:value-of select="'MEMOFIELD'"/>
        </MEMOFIELD>

        <CURRENCYCODE>
          <xsl:value-of select="'CURRENCYCODE'"/>
        </CURRENCYCODE>

       


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

   <xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='Equity') and CurrencySymbol!='USD' and CounterParty!='VCGO']"> 

      <!-- <xsl:for-each select="ThirdPartyFlatFileDetail"> -->
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <ACCOUNT>
            <xsl:value-of select="'A45C1209'"/>
          </ACCOUNT>

          <SIDE>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SIDE>


          <QUANTITY>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </QUANTITY>


          <SEDOL>
            <xsl:value-of select="concat(SEDOL,'.')"/>
          </SEDOL>

          <PRICE>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PRICE>

        

          <xsl:variable name="varPershare">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
          <COMMISSION>
            <xsl:choose>
              <xsl:when test="CommissionCharged &lt;1">
                <xsl:value-of select="format-number($varPershare,'##.##')"/>
              </xsl:when>
              <xsl:when test="CommissionCharged &gt;1">
                <xsl:value-of select="concat('c',$varCommission)"/>
              </xsl:when>
            </xsl:choose>
          </COMMISSION>

           <BrokerCode>
            <xsl:choose>
              <xsl:when test="MEMOFIELD='AL@C' and AccountName='Multi Strat Endurant AVL5'">
                <xsl:value-of select="'AVL51209'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD='AL@C' and AccountName='Multi Strat GS Main AVE5'">
                <xsl:value-of select="'AVE51209'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD='AL@C' and AccountName='Multi Strat Leucadia Kathmandu AVML'">
                <xsl:value-of select="'AVML1209'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD='AL@C' and AccountName='Multi Strat GS Main AVE5'">
                <xsl:value-of select="'AVE51209'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD='AL@C' and AccountName='Multi Strat GS Main AVE5'">
                <xsl:value-of select="'AVE51209'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD='AL@C'">
                <xsl:value-of select="AccountName"/>
              </xsl:when>
			   <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='COWN'">
                <xsl:value-of select="'039160775'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='ICBX'">
                <xsl:value-of select="'039121389'"/>
              </xsl:when>
			 <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='MZHO'">
                <xsl:value-of select="'039132279'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='TDSI'">
                <xsl:value-of select="'039123575'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='MSCO'">
                <xsl:value-of select="'039124169'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='GSCO'">
                <xsl:value-of select="'039122700'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='RBCA'">
                <xsl:value-of select="'039810999'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='BMOC'">
                <xsl:value-of select="'039162839'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='PIPR'">
                <xsl:value-of select="'039143185'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='LEHM'">
                <xsl:value-of select="'039141346'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='SCMC'">
                <xsl:value-of select="'039148051'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='PIPR'">
                <xsl:value-of select="'039148010'"/>
              </xsl:when>
			  <xsl:when test="MEMOFIELD!='AL@C' and CounterParty='COWN'">
                <xsl:value-of select="'039160775'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>           
          </BrokerCode>


          <TRADEDATEYEAR>
            <xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
          </TRADEDATEYEAR>

          <TRADEDATEMONTH>
            <xsl:value-of select="substring-before(TradeDate,'/')"/>
          </TRADEDATEMONTH>

          <TRADEDATEDAY>
            <xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
          </TRADEDATEDAY>

          <SETTLEDATEYEAR>
            <xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
          </SETTLEDATEYEAR>

          <SETTLEDATEMONTH>
            <xsl:value-of select="substring-before(SettlementDate,'/')"/>
          </SETTLEDATEMONTH>

          <SETTLEDATEDAY>
           <xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
          </SETTLEDATEDAY>

          <MEMOFIELD>
            <xsl:value-of select="MEMOFIELD"/>
          </MEMOFIELD>

          <CURRENCYCODE>
            <xsl:choose>
              <xsl:when test="CurrencySymbol='USD'">
                <xsl:value-of select="'1'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='GBP'">
                <xsl:value-of select="'2'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='JPY'">
                <xsl:value-of select="'3'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='MXN'">
                <xsl:value-of select="'4'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='CHF'">
                <xsl:value-of select="'5'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='EUR'">
                <xsl:value-of select="'6'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='CAD'">
                <xsl:value-of select="'7'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='NZD'">
                <xsl:value-of select="'8'"/>
              </xsl:when>

              <xsl:when test="CurrencySymbol='AUD'">
                <xsl:value-of select="'10'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='HKD'">
                <xsl:value-of select="'11'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='DKK'">
                <xsl:value-of select="'12'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='CZK'">
                <xsl:value-of select="'13'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='HUF'">
                <xsl:value-of select="'14'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='NOK'">
                <xsl:value-of select="'15'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='PLN'">
                <xsl:value-of select="'16'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol='SEK'">
                <xsl:value-of select="'17'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'0'"/>
              </xsl:otherwise>
            </xsl:choose>
          </CURRENCYCODE>
		  
		 
		  
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>