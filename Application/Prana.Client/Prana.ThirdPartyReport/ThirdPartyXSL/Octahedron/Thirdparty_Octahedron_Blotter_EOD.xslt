<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'Jan'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'Feb'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'Mar'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'Apr'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'May'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'Jun'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'Jul'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'Aug'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'Sep'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'Oct'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'Nov'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'Dec'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
  </xsl:template>
  
  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    
    <xsl:variable name="varMonth">
      <xsl:call-template name="MonthCodevar">
        <xsl:with-param name="Month" select="substring-before($Date,'/')"/>
      </xsl:call-template>
    </xsl:variable>
    
    <xsl:variable name="varDay">
      <xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
    </xsl:variable>
    <xsl:value-of select="concat($varDay,'-',$varMonth)"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <TD>
          <xsl:value-of select="'TD'"/>
        </TD>

        <SD>
          <xsl:value-of select="'SD'"/>
        </SD>

        <TRX>
          <xsl:value-of select="'TRX'"/>
        </TRX>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <QUANT>
          <xsl:value-of select="'QUANT'"/>
        </QUANT>

        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <COMM>
          <xsl:value-of select="'COMM'"/>
        </COMM>

        <NETPRICE>
          <xsl:value-of select="'NET PRICE'"/>
        </NETPRICE>

        <GROSS>
          <xsl:value-of select="'GROSS $'"/>
        </GROSS>

        <NET>
          <xsl:value-of select="'NET $'"/>
        </NET>

        <BROKER>
          <xsl:value-of select="'BROKER'"/>
        </BROKER>

        <PB>
          <xsl:value-of select="'PB'"/>
        </PB>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate"/>
            </xsl:call-template>
          </xsl:variable>
          <TD>
            <xsl:value-of select="$varTradeDate"/>
          </TD>

          <xsl:variable name="varSettlementDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate"/>
            </xsl:call-template>
          </xsl:variable>
          <SD>
            <xsl:value-of select="$varSettlementDate"/>
          </SD>

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>

              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BY'"/>
              </xsl:when>

              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
			  
			  <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <TRX>
            <xsl:value-of select="$varSide"/>
          </TRX>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <xsl:variable name="varQuantity">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <QUANT>
            <xsl:value-of select="$varQuantity"/>
          </QUANT>

          <xsl:variable name="varAvgPrice">
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <PRICE>
            <xsl:value-of select="format-number($varAvgPrice,'#.####')"/>
          </PRICE>

          <xsl:variable name="varCommission">
            <xsl:choose>
              <xsl:when test="number(CommissionCharged)">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <COMM>
            <xsl:value-of select="$varCommission"/>
          </COMM>

          <xsl:variable name="varNetNotional">
            <xsl:choose>
              <xsl:when test="number(NetAmount)">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varNetPrice">
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="($varNetNotional div $varQuantity) div 100"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="$varNetNotional div $varQuantity"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <NETPRICE>
            <xsl:value-of select="format-number($varNetPrice,'#.####')"/>
          </NETPRICE>

          <xsl:variable name="varGross">
            <xsl:choose>
              <xsl:when test="number(GrossAmount)">
                <xsl:value-of select="GrossAmount"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <GROSS>
            <xsl:value-of select="format-number($varGross,'#.##')"/>
          </GROSS>

          <NET>
            <xsl:value-of select="format-number($varNetNotional,'#.##')"/>
          </NET>

          <BROKER>
            <xsl:value-of select="CounterParty"/>
          </BROKER>

          <xsl:variable name="PB_NAME" select="'Jone'"/>
          
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_PB_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_PBAccountMapping.xml')/PBAccountMapping/PB[@Name=$PB_NAME]/PBData[@PranaFund=$PRANA_FUND_NAME]/@PB"/>
          </xsl:variable>
          <PB>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_PB_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_PB_NAME"/>
              </xsl:when>
			   <xsl:when test="AccountName='Octahedron Fund'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron Onshore Fund, L.P'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron Offshore Fund, Ltd'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron MS Swap'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='MSPA In House'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron Fund IPO'">
                <xsl:value-of select="'MS'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron Northern Trust Bank'">
                <xsl:value-of select="'NT'"/>
              </xsl:when>
			  	   <xsl:when test="AccountName='Octahedron Fund GS'">
                <xsl:value-of select="'GS'"/>
              </xsl:when>
			    <xsl:when test="AccountName='Octahedron Fund GS Swap'">
                <xsl:value-of select="'GS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </PB>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>