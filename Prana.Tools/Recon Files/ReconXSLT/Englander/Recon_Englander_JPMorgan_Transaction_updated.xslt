<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth=01">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=02">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=03">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=04">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=05">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=06">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=07">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=08">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=09">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=10">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=11">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=12">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=13">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=14">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=15">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=16">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=17">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=18">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=19">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=20">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=21">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=22">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=23">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=24">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="Comparision">

        <xsl:variable name ="varPrice">
          <xsl:value-of select ="substring(COL1,137,16)"/>
        </xsl:variable>


        <xsl:variable name ="varFormatPrice">
          <xsl:value-of select="concat(substring($varPrice,1,8),'.',substring($varPrice,9,8))"/>
        </xsl:variable>

        <xsl:if test="substring(COL1,4,5)='87017' or substring(COL1,4,5)='87010'">
          <PositionMaster>

            <xsl:variable name="varFund" select="substring(COL1,4,5)"/>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:if test ="$varFund != ''">
                <xsl:value-of select="$varFund"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPMorgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME =''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>


            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,39,30))"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='NewLand_GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>


            <xsl:variable name ="varSymbol">
              <xsl:value-of select ="translate(normalize-space(substring(COL1,25,8)),' ','')"/>
            </xsl:variable>

            <xsl:variable name="varUnderlying" select="translate(normalize-space(substring(COL1,241,8)),' ','')"/>

            <xsl:variable name="varOptExpiration_Year" select="substring(COL1,845,2)" />
            <xsl:variable name="varOptExpiration_Month" select="substring(COL1,847,2)" />
            <xsl:variable name="Strike_PriceInt" select="number(substring(COL1,224,8))"/>
            <xsl:variable name="Strike_Price" select="concat($Strike_PriceInt,'.',substring(COL1,232,2))"/>

            <xsl:variable name ="varCallPutCode">
              <xsl:value-of select="substring(COL1,841,1)"/>
            </xsl:variable>

            <xsl:variable name = "varMonthCode" >
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="$varOptExpiration_Month" />
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varStrike">
              <xsl:choose>
                <xsl:when test="$varCallPutCode !=' '">
                  <xsl:value-of select="$Strike_Price"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test ="$varCallPutCode !='' and $varUnderlying != ''">
                  <xsl:value-of select="concat('O:',$varUnderlying,' ',$varOptExpiration_Year,$varMonthCode,$varStrike)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSymbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <PBSymbol>
              <xsl:value-of select="$varSymbol"/>
            </PBSymbol>

            <PBAssetName>
              <xsl:value-of select="''"/>
            </PBAssetName>

            <!--Side-->
            <xsl:variable name ="varSide">
              <xsl:value-of select="normalize-space(substring(COL1,94,1))"/>
            </xsl:variable>

            <Side>
              <xsl:choose>
                <xsl:when test="$varSide='B' and $varCallPutCode =''">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$varSide='S'and  $varCallPutCode =''">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>

                <xsl:when test="$varSide='B' and $varCallPutCode !=''">
                  <xsl:value-of select="'Buy to Open'"/>
                </xsl:when>
                <xsl:when test="$varSide='S'  and $varCallPutCode !=''">
                  <xsl:value-of select="'Sell to Close'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>


            <xsl:variable name ="varNetPosition">
              <xsl:value-of select ="substring(COL1,118,18)"/>
            </xsl:variable>


            <xsl:variable name ="varFormatQty">
              <xsl:value-of select="concat(substring($varNetPosition,1,13),'.',substring($varNetPosition,14,5))"/>
            </xsl:variable>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varFormatQty)">
                  <xsl:value-of select="format-number($varFormatQty, '###,###,###.00000')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <AvgPX>
              <xsl:choose>
                <xsl:when test="number($varFormatPrice)">
                  <xsl:value-of select='format-number($varFormatPrice, "###,###,###.########")'/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>


            <xsl:variable name="varCommissionstring" select="substring(COL1,155,13)"/>

            <Commission>
              <xsl:choose>
                <xsl:when test ="number($varCommissionstring)">
                  <xsl:value-of select="concat(substring($varCommissionstring,1,11),'.',substring($varCommissionstring,12,2))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varFeesstring" select="substring(COL1,170,11)"/>

            <Fees>
              <xsl:choose>
                <xsl:when test ="number($varFeesstring)">
                  <xsl:value-of select="concat(substring($varFeesstring,1,9),'.',substring($varFeesstring,10,2))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
