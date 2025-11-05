<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
        <xsl:when test="$Month=07 ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08 ">
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


  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>
    <xsl:variable name="varUSymbol">
      <xsl:value-of select="substring-before(COL3,' ')"/>
    </xsl:variable>

    <xsl:variable name="varPutORCall">
      <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL3),' '),' '),9,1)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL3),' '),' '),3,2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring-after(substring-after(normalize-space(COL3),' '),' '),'#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL3),' '),' '),7,2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL3),' '),' '),5,2)"/>
    </xsl:variable>


    <xsl:variable name="MonthCodeVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$varMonthCode"/>
        <xsl:with-param name="PutOrCall" select="$varPutORCall"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="varExpiryDay">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)= '0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="normalize-space(concat('O:',$varUSymbol,' ', $varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL34"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($Position) and (COL32='Buy' or COL32='Sell')">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Abdiel'"/>
            </xsl:variable>
            
            <xsl:variable name = "PB_COMPANY_NAME" >
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

           
            

            <xsl:variable name="varCusip">
              <xsl:value-of select="normalize-space(COL16)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

				  <xsl:when test="$varCusip !='' or $varCusip !='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

              

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCusip !='' or $varCusip !='*'">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>

             

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>


            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

			<xsl:variable name="PB_CountnerParty" select="''"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:value-of select="'125'"/>
            </CounterPartyID>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL35"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when  test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL41"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when  test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring-before(substring-after(COL26,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring-after(substring-after(COL26,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring-before(COL26,'-')"/>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="COL26"/>
            </PositionStartDate>

            <xsl:variable name="varSDay">
              <xsl:value-of select="substring-before(substring-after(COL27,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varSYear">
              <xsl:value-of select="substring-after(substring-after(COL27,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring-before(COL27,'-')"/>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="COL27"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when  test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>


            <SideTagValue>
              <xsl:choose>                
                    <xsl:when  test="COL32='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when  test="COL32='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>                 
              </xsl:choose>
            </SideTagValue>



            <OriginalPurchaseDate>
              <xsl:value-of select="''"/>
            </OriginalPurchaseDate>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


