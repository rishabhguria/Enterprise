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

  <xsl:template match="/">
    <DocumentElement>
	

		<xsl:for-each select ="//Comparision">	
		  
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>

			<xsl:variable name="varSide">
				<xsl:value-of select="COL12"/>
			</xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and ($varSide='BUY' or $varSide='SELL')">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL16"/>
              </xsl:variable>

				<xsl:variable name="varSymbol">
					<xsl:value-of select="COL15"/>
				</xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME or @PBCompanyName=$varSymbol]/@PranaSymbol"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <xsl:variable name="PB_FUND_NAME" select="COL2"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>

                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="$Quantity * -1"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL21"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValue &gt; 0">
                    <xsl:value-of select="$NetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValue &lt; 0">
                    <xsl:value-of select="$NetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>
			  
			  <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="0"/>
                </xsl:call-template>
              </xsl:variable>
			  
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$NetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>
				
			    	<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL18"/>
								</xsl:call-template>
							</xsl:variable>
							<AvgPX>
								<xsl:choose>
									<xsl:when test="$AvgPrice &gt; 0">
										<xsl:value-of select="$AvgPrice"/>
									</xsl:when>
									<xsl:when test="$AvgPrice &lt; 0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPX>


							<xsl:variable name="varCommission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>
							<Commission>
								<xsl:choose>
									<xsl:when test="$varCommission &gt; 0">
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



				<xsl:variable name = "varTradeDay" >
					<xsl:value-of select="substring-after(substring-after(COL9,'-'),'-')"/>
				</xsl:variable>

				<xsl:variable name = "varTradeMonth" >
					<xsl:value-of select="substring-before(substring-after(COL9,'-'),'-')"/>
				</xsl:variable>

				<xsl:variable name = "varTradeYear" >
					<xsl:value-of select="substring-before(COL9,'-')"/>
				</xsl:variable>

				<xsl:variable name = "varTradeDate" >
					<xsl:value-of select="concat($varTradeMonth,'/',$varTradeDay,'/',$varTradeYear)"/>
				</xsl:variable>

				<TradeDate>
					<xsl:value-of select="$varTradeDate"/>
				</TradeDate>


				<xsl:variable name = "varSettleMonth" >
					<xsl:value-of select="substring-before(substring-after(COL10,'-'),'-')"/>
				</xsl:variable>
				<xsl:variable name = "varSettleYear" >
					<xsl:value-of select="substring-before(COL10,'-')"/>
				</xsl:variable>

				<xsl:variable name = "varSettleDay" >
					<xsl:value-of select="substring-after(substring-after(COL10,'-'),'-')"/>
				</xsl:variable>

				<xsl:variable name = "varSettleDate" >
					<xsl:value-of select="concat($varSettleMonth,'/',$varSettleDay,'/',$varSettleYear)"/>
				</xsl:variable>

				<SettlementDate>
					<xsl:value-of select="$varSettleDate"/>
				</SettlementDate>


            
              <Side>
                <xsl:choose>
                  <xsl:when test="$varSide ='BUY'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="$varSide ='SELL'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

				
            
              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

            </PositionMaster>
          </xsl:when>

          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select ="''"/>
              </Symbol>

              <FundName>
                <xsl:value-of select ="''"/>
              </FundName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>
			  
              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>
				
				
				<AvgPX>
					  <xsl:value-of select="0"/>				
				</AvgPX>
				
				<Commission>
					  <xsl:value-of select="0"/>				
				</Commission>
				

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>
				
				<SettlementDate>
					 <xsl:value-of select ="''"/>
				</SettlementDate>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


