<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space(COL9),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before(substring-after(normalize-space(COL9),'/'),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL9),' '),' '),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL9),'/'),'/'),' ')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL9),'/'),'/'),' '),' '),2),'#.00')"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL9),'/'),'/'),' '),' '),1,1)"/>
    </xsl:variable>
    <xsl:variable name="MonthCodeVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
        <xsl:with-param name="PutOrCall" select="$PutORCall"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="substring($ExpiryDay,1,1)='0'">
          <xsl:value-of select="substring($ExpiryDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
  </xsl:template>
  
  
  
  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL37"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varDividendBase">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL38"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:choose>
          <xsl:when test="number($varDividend) or number($varDividendBase)">
            <PositionMaster>
              <xsl:variable name="PB_Name">
                <xsl:value-of select="'HedgeServ'"/>
              </xsl:variable>
              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="normalize-space(COL3)"/>
              </xsl:variable>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>


           

              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL9)"/>
              </xsl:variable>
				<xsl:variable name="varSymbolHavingCFD">
                <xsl:value-of select="concat(substring-before(normalize-space(COL9),'EQUITY'),'EQUITY')"/>
              </xsl:variable>
			  <xsl:variable name="varSymbolOption">
               <xsl:choose>
				<xsl:when test="contains(COL8,'.')">
                    <xsl:value-of select="concat(normalize-space(COL8),' EQUITY')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="concat(normalize-space(COL8),' EQUITY')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>


               <Symbol>
                <xsl:choose>
				
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
				<xsl:when test="contains(COL9,'(CFD)')">
                    <xsl:value-of select="$varSymbolHavingCFD"/>
                  </xsl:when>
				<xsl:when test="COL6='Option'">
                    <xsl:value-of select="$varSymbolOption"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

            

              <DividendBase>
                <xsl:choose>
                  <xsl:when test="number($varDividendBase)">
                    <xsl:value-of select="$varDividendBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </DividendBase>

              <DividendLocal>
                <xsl:choose>
                  <xsl:when test="number($varDividend)">
                    <xsl:value-of select="$varDividend"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </DividendLocal>
			  
			  <xsl:variable name="varEndingQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>
			  <EndingQuantity>
                <xsl:choose>
                  <xsl:when test="number($varEndingQuantity)">
                    <xsl:value-of select="$varEndingQuantity"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </EndingQuantity>

              <xsl:variable name="varTotalCost">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <TotalCostLocal>
                <xsl:choose>
                  <xsl:when test="number($varTotalCost)">
                    <xsl:value-of select="$varTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCostLocal>

              <TotalCostBase>
                <xsl:choose>
                  <xsl:when test="number($varTotalCost)">
                    <xsl:value-of select="$varTotalCost"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCostBase>

              <SecurityName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </SecurityName>
			  
			  <xsl:variable name="varUDASubSector">
                <xsl:value-of select="COL6"/>
              </xsl:variable>
			    <UDASubSector>
                <xsl:value-of select="$varUDASubSector"/>
              </UDASubSector>
			  
			  <xsl:variable name="varUDASector">
                <xsl:value-of select="COL8"/>
              </xsl:variable>
			    <UDASector>
                <xsl:value-of select="$varUDASector"/>
              </UDASector>


            </PositionMaster>
          </xsl:when>

          <xsl:otherwise>
            <PositionMaster>
              <PortfolioAccount>
                <xsl:value-of select ="''"/>
              </PortfolioAccount>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
			  
			   <UDASubSector>
                <xsl:value-of select="''"/>
              </UDASubSector>
			  
			  <UDASector>
                <xsl:value-of select="''"/>
              </UDASector>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <DividendBase>
                <xsl:value-of select="0"/>
              </DividendBase>

              <DividendLocal>
                <xsl:value-of select="0"/>
              </DividendLocal>
			  
			   <EndingQuantity>
                <xsl:value-of select="0"/>
              </EndingQuantity>

              <TotalCostLocal>
                <xsl:value-of select="0"/>
              </TotalCostLocal>

              <TotalCostBase>
                <xsl:value-of select="0"/>
              </TotalCostBase>

              <SecurityName>
                <xsl:value-of select="''"/>
              </SecurityName>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
