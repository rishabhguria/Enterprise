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

      <xsl:for-each select="ThirdPartyFlatFileDetail[CurrencySymbol='USD' and Asset!='EquityOption']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxlotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxlotState>

          <xsl:variable name="PB_NAME" select="'Telsey'"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>



          <ACCOUNT>
            <xsl:choose>
              <xsl:when test="AccountName='Telsey'">
                <xsl:value-of select="'761-110775'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>


          </ACCOUNT>

			<TYPE>
				<!--<xsl:value-of select="concat(concat('=&quot;','0608'),'&quot;')"/>-->
				<xsl:value-of select="'Margin'"/>


			</TYPE>
          
          
          <SIDE>

            <xsl:choose>
				<xsl:when test="contains(Side,'Buy to Close')">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				
              <xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="contains(Side,'Sell short')">
                <xsl:value-of select="'SS'"/>
              </xsl:when>

              <xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="'S'"/>
              </xsl:when>

				
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


          <SYMBOL>

            <xsl:value-of select="UnderlyingSymbol"/>
          </SYMBOL>

          

          <PRICE>

			  <xsl:value-of select="format-number(AveragePrice,'#.####')"/>
		  </PRICE>

          <DONEAWAYCOMM>
			  <xsl:choose>
				  <xsl:when test="CommissionCharged &lt; 1">
					  <xsl:value-of select="concat('c',format-number(CommissionCharged,'0.##'))"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="format-number(CommissionCharged,'0.##')"/>
				  </xsl:otherwise>
			  </xsl:choose>

            
          </DONEAWAYCOMM>
			<Date>
				<xsl:value-of select="TradeDate"/>
			</Date>

          <!--<BROKER>
            --><!--<xsl:value-of select="concat(concat('=&quot;','0608'),'&quot;')"/>--><!--
            --><!--<xsl:value-of select="''"/>--><!--


          </BROKER>-->

          <!--<FL1>
            <xsl:value-of select="''"/>
          </FL1>

          <FL2>
            <xsl:value-of select="''"/>
          </FL2>

          <FL3>
            <xsl:value-of select="''"/>
          </FL3>

			<Month>
				<xsl:value-of select="substring-before(TradeDate,'/')"/>
			</Month>

			<Date>
				<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
			</Date>

			<Year>
				<xsl:value-of select="substring(substring-after(substring-after(TradeDate,'/'),'/'),3,4)"/>
			</Year>-->

         
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>