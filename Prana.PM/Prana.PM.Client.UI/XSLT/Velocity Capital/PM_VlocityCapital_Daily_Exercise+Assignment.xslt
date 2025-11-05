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
      <xsl:value-of select="substring-before($varBBCode,' ')"/>
    </xsl:variable>

    <xsl:variable name="varPutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),7,1)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),1,2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),5,2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),3,2)"/>
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
    <xsl:value-of select="normalize-space(concat('O:',$varUSymbol,' ',$varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
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
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($Position) and normalize-space(COL3)='OPEXP' or normalize-space(COL3)='OPXRC' or normalize-space(COL3)='OPASN'">
          <!--<xsl:if test="number($Position) and (COL32='Buy' or COL32='Sell') and COL9!='CASH'">-->
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Apex'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY_NAME" >
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="string-length(COL8) &gt; 10">
                  <xsl:call-template name="ConvertBBCodetoTicker">
                    <xsl:with-param name="varBBCode" select="COL8"/>
                  </xsl:call-template>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>




            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>


                 <xsl:variable name="PB_FUND_NAME" select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

              <!--<xsl:value-of select ="'Papyrus Capital Fund LP'"/>-->
            </AccountName>



            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                

                <xsl:when test="string-length(COL8) &gt; 10">
                 <xsl:value-of select="'0'"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>


            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when  test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
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
			
			  <xsl:variable name="varOrffees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>
            <OrfFee>
              <xsl:choose>
                <xsl:when  test="$varOrffees &gt; 0">
                  <xsl:value-of select="$varOrffees"/>
                </xsl:when>
                <xsl:when test="$varOrffees &lt; 0">
                  <xsl:value-of select="$varOrffees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>
			
			  <xsl:variable name="COL7">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
              </xsl:call-template>
            </xsl:variable>
			
			<xsl:variable name="Fees1">
              <xsl:choose>
			
				
				  <xsl:when  test="string-length(COL8) &lt; 10 and  $Position &gt; 0">
                        <xsl:value-of select=" ($COL7 -(($Position * $varCostBasis ) + $varCommission + $varSecFee + $varOrffees))"/>
                </xsl:when >
				<xsl:when  test="string-length(COL8) &lt; 10 and  $Position &lt; 0">
                     <xsl:value-of select=" ($COL7 -(($Position * $varCostBasis) + $varCommission + $varSecFee + $varOrffees))"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			
            <Fees>
              
                  <xsl:value-of select ="$Fees1"/>
                
            </Fees>
			
			<OptionPremiumAdjustment>
			  <xsl:choose>
			  <xsl:when  test="string-length(COL8) &gt; 10 and contains(COL3,'OPEXP')">
                  <xsl:value-of select="'0'"/>
                </xsl:when >
                <xsl:when  test="string-length(COL8) &gt; 10">
                  <xsl:value-of select="'0'"/>
                </xsl:when >
				 <xsl:when  test="string-length(COL8) &lt; 10">
                    <xsl:value-of select="'0'"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </OptionPremiumAdjustment>


            <xsl:variable name="varDay">
              <xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring-before(COL5,'/')"/>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </PositionStartDate>

            <xsl:variable name="varSDay">
              <xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varSYear">
              <xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring-before(COL6,'/')"/>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="concat($varSMonth,'/',$varSDay,'/',$varSYear)"/>
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
                 <xsl:when  test="string-length(COL8) &gt; 10 and COL9 &gt; 0">
                  <xsl:value-of select="'B'"/>
                </xsl:when >
				<xsl:when  test="string-length(COL8) &gt; 10 and COL9 &lt; 0">
                  <xsl:value-of select="'D'"/>
                </xsl:when >
                <xsl:when  test="string-length(COL8) &lt; 10 and COL9 &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when >
				<xsl:when  test="string-length(COL8) &lt; 10 and COL9 &lt; 0">
                  <xsl:value-of select="'2'"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
			
				<TransactionType>
              <xsl:choose>
                <xsl:when  test="contains(COL3,'OPASN')">
                  <xsl:value-of select="'Assignment'"/>
                </xsl:when>
                 <xsl:when  test="contains(COL3,'OPXRC')">
                  <xsl:value-of select="'Exercise'"/>
                </xsl:when>
				 <xsl:when  test="contains(COL3,'OPEXP')">
                  <xsl:value-of select="'Expire'"/>
                </xsl:when>
				
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionType>
			
			 <xsl:variable name="PB_CountnerParty" select="normalize-space(COL27)"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="contains(COL27,'ARCA')">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="contains(COL27,'CHBC')">
                  <xsl:value-of select="'7'"/>
                </xsl:when>
				<xsl:when test="contains(COL27,'DEAN')">
                  <xsl:value-of select="'9'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'DFIN')">
                  <xsl:value-of select="'11'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'ETRS')">
                  <xsl:value-of select="'12'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'FOGS')">
                  <xsl:value-of select="'14'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'NON')">
                  <xsl:value-of select="'15'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'OCC')">
                  <xsl:value-of select="'16'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'PUMX')">
                  <xsl:value-of select="'18'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'QNTX')">
                  <xsl:value-of select="'19'"/>
                </xsl:when>
				<xsl:when test="contains(COL27,'WEXM')">
                  <xsl:value-of select="'20'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'RDBN')">
                  <xsl:value-of select="'13'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'ISIG')">
                  <xsl:value-of select="'17'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'WELX')">
                  <xsl:value-of select="'19'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'MLCO')">
                  <xsl:value-of select="'22'"/>
                </xsl:when>
				
			
                <xsl:when test="$PRANA_CounterPartyID !=''">
                  <xsl:value-of select ="$PRANA_CounterPartyID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>



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


