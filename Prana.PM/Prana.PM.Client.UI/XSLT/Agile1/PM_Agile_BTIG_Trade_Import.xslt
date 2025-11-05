<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
		  <xsl:if test ="COL1 != 'Fund' and COL9 != 'Cash'">

			  <PositionMaster>
				  <!--   Fund -->
				  <!--fundname section-->
				  <xsl:variable name = "PB_FUND_NAME">
					  <xsl:value-of select="COL1"/>
				  </xsl:variable>
				  
				  <xsl:variable name="PRANA_FUND_NAME">
					  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
				  </xsl:variable>

				  <!--<xsl:variable name = "PB_SymbolCurrency_NAME" >
					  <xsl:value-of select="COL3"/>
				  </xsl:variable>

				  <xsl:variable name = "PB_Currency_NAME" >
					  <xsl:value-of select="COL20"/>
				  </xsl:variable>

				  <xsl:variable name="PRANA_SymbolCurrency_NAME">
					  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@CompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
				  </xsl:variable>-->

				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME=''">
						  <AccountName>
							  <xsl:value-of select='$PB_FUND_NAME'/>
						  </AccountName>
					  </xsl:when>
					  <xsl:otherwise>
						  <AccountName>
							  <xsl:value-of select='$PRANA_FUND_NAME'/>
						  </AccountName>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test ="COL17 = 'Trade Date' or COL17 = '*'">
						  <PositionStartDate>
							  <xsl:value-of select="''"/>
						  </PositionStartDate>
					  </xsl:when>
					  <xsl:otherwise>
						  <PositionStartDate>
							  <xsl:value-of select="COL17"/>
						  </PositionStartDate>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:variable name="PB_Symbol" select="COL8"/>
				  <xsl:variable name="PRANA_SYMBOL_NAME">
					  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
				  </xsl:variable>

				  <!--<Symbol>
				  <xsl:choose>
					  <xsl:when test="$PB_Symbol != ''">
						  <xsl:value-of select="$PB_Symbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL5"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>-->

				  <xsl:choose>
					  <xsl:when test ="contains(COL4,'Equities') != false">
						  <Symbol>
							  <xsl:value-of select="COL5"/>
						  </Symbol>
						  <IDCOOptionSymbol>
							  <xsl:value-of select="''"/>
						  </IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="contains(COL4,'Options') != false">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
							  <xsl:value-of select="concat(COL5,'U')"/>
						  </IDCOOptionSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
					  </xsl:otherwise>
				  </xsl:choose>
				  <!--<Symbol>
					  <xsl:value-of select="COL5"/>
				  </Symbol>-->
				  
				  <PBSymbol>
					  <xsl:value-of select="COL5"/>
				  </PBSymbol>

				  <!--QUANTITY-->

				  <xsl:choose>
				  <xsl:when test="COL10 &lt; 0">
					  <NetPosition>
						  <xsl:value-of select="COL10 * (-1)"/>
					  </NetPosition>
				  </xsl:when>
				  <xsl:when test="COL10 &gt; 0">
					  <NetPosition>
						  <xsl:value-of select="COL10"/>
					  </NetPosition>
				  </xsl:when>
				  <xsl:otherwise>
					  <NetPosition>
						  <xsl:value-of select="0"/>
					  </NetPosition>
				  </xsl:otherwise>
			  </xsl:choose>

			  <!--Side-->
			  <xsl:choose>

				  <xsl:when test="COL9='Long'">
					  <SideTagValue>
							  <xsl:value-of select="'1'"/>
						  </SideTagValue>
					  </xsl:when>
					  <xsl:when test="COL9='Short'">
						  <SideTagValue>
							  <xsl:value-of select="'5'"/>
						  </SideTagValue>
					  </xsl:when>					  
					  <xsl:otherwise>
						  <SideTagValue>
							  <xsl:value-of select="''"/>
						  </SideTagValue>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:choose>
					  <xsl:when test ="number(COL11)">
						  <CostBasis>
							  <xsl:value-of select="COL11"/>
						  </CostBasis>
					  </xsl:when>
					  <xsl:otherwise>
						  <CostBasis>
							  <xsl:value-of select="0"/>
						  </CostBasis>
					  </xsl:otherwise>
				  </xsl:choose>

				  <!--<xsl:choose>
            <xsl:when test ="number(COL5) and number(COL5) != 0 and number(COL9)">
              <CostBasis>
                <xsl:value-of select="(COL9 div COL5) * -1"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>-->

				  <!--<xsl:choose>
					  <xsl:when test="COL7&gt; 0">
						  <Commission>
							  <xsl:value-of select="COL7"/>
						  </Commission>
					  </xsl:when>
					  <xsl:when test="COL7 &lt; 0">
						  <Commission>
							  <xsl:value-of select="COL7*(-1)"/>
						  </Commission>
					  </xsl:when>
					  <xsl:otherwise>
						  <Commission>
							  <xsl:value-of select="0"/>
						  </Commission>
					  </xsl:otherwise>
				  </xsl:choose>

				  <xsl:variable name ="varMiscSum">
					  <xsl:value-of select ="COL8 + COL9 + COL10 "/>
				  </xsl:variable>

				  <xsl:choose>
					  <xsl:when test="$varMiscSum &lt; 0">
						  <MiscFees>
							  <xsl:value-of select="$varMiscSum*(-1)"/>
						  </MiscFees>
					  </xsl:when>
					  <xsl:when test="$varMiscSum &gt; 0">
						  <MiscFees>
							  <xsl:value-of select="$varMiscSum"/>
						  </MiscFees>
					  </xsl:when>
					  <xsl:otherwise>
						  <MiscFees>
							  <xsl:value-of select="0"/>
						  </MiscFees>
					  </xsl:otherwise>
				  </xsl:choose>

				  <CounterPartyID>
					  <xsl:value-of select="'16'"/>
				  </CounterPartyID>-->
				  <FXRate>
					  <xsl:choose>
						  <xsl:when test ="number(COL18)">
							  <xsl:value-of select ="COL18"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="0"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </FXRate>

				  <FXConversionMethodOperator>
					  <xsl:value-of select ="'M'"/>
				  </FXConversionMethodOperator>
				  
			  </PositionMaster>
		  </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
