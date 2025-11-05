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

      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'AKL70409' or AccountName = 'AKL80409' or AccountName = 'AKL50409' or AccountName = 'AKL60409']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxlotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxlotState>

          <xsl:variable name="PB_NAME" select="'WH'"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>



          <ACCOUNT>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>

          </ACCOUNT>

          <SYMBOL>
            <!--<xsl:value-of select="Symbol"/>-->
			  <xsl:value-of select="UnderlyingSymbol"/>
		  </SYMBOL>

          <SIDE>
            <xsl:choose>
              <!--<xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="'B'"/>
              </xsl:when>-->

              <xsl:when test="contains(Side,'Sell to Open')">
                <xsl:value-of select="'SS'"/>
              </xsl:when>

				<xsl:when test="contains(Side,'Sell to Close')">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="contains(Side,'Buy to Close')">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				<xsl:when test="contains(Side,'Buy to Open')">
					<xsl:value-of select="'B'"/>
				</xsl:when>

              <!--<xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="'S'"/>
              </xsl:when>-->
            </xsl:choose>

          </SIDE>

          <QUANTITY>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>

          </QUANTITY>

          <PRICE>
            <xsl:value-of select='format-number(AveragePrice, "0.000000")'/>

          </PRICE>

          <BROKER>
			  <xsl:choose>
				  <xsl:when test="CounterParty='WEX' and Asset='EquityOption'">
					  <xsl:value-of select="'0365'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'0659'"/>
				  </xsl:otherwise>
			  </xsl:choose>
            

          </BROKER>

          <EXPYEAR>
            <xsl:value-of select="substring(substring-after(substring-after(ExpirationDate,'/'),'/'), 3, 2)"/>

          </EXPYEAR>

          <EXPMONTH>
            <xsl:value-of select="substring-before(ExpirationDate,'/')"/>

          </EXPMONTH>

          <EXPDAY>
            <xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>

          </EXPDAY>

          <PUTCALL>
            <!--<xsl:value-of select="PutOrCall"/>-->
			  <xsl:choose>
				  <xsl:when test="PutOrCall='PUT'">
					  <xsl:value-of select="'P'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'C'"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </PUTCALL>

          <STRIKEPRICE>
            <xsl:value-of select="StrikePrice"/>

          </STRIKEPRICE>

		  <xsl:variable name="varTradeDateTime" select="TradeDateTime"/>
		  <xsl:variable name="varTradeTime" select="substring-after($varTradeDateTime, ' ')"/>
			<xsl:variable name="varH" select="substring-before($varTradeTime, ':')"/>
			<xsl:variable name="varHH">
				<xsl:choose>
					<xsl:when test="contains(TradeDateTime,'PM') and $varH != 12">
						<xsl:value-of select="format-number($varH + 12, '00')"/>
					</xsl:when>
					<xsl:when test="contains(TradeDateTime,'AM') and $varH = 12">
						<xsl:value-of select="format-number(00, '00')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number($varH, '00')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<EXCHOUR>
				<xsl:value-of select="$varHH"/>

			</EXCHOUR>

          <EXCMIN>
			  <xsl:value-of select="substring-before(substring-after($varTradeTime, ':'), ':')"/>

          </EXCMIN>

          <EXCSEC>
			  <xsl:value-of select="substring(substring-after(substring-after($varTradeTime, ':'), ':'), 1, 2)"/>

          </EXCSEC>




          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>