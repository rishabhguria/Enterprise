<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'$',''),$SingleQuote,''))"/>
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

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varPosition)">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'US Bank'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Blotter']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

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
            </AccountName>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>
			  
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>
            </PBSymbol>

			 <xsl:variable name="varCost">
			  <xsl:choose>
                <xsl:when test ="COL10 = 0">
                  <xsl:value-of select ="COL11"/>
                </xsl:when>
                <xsl:when test="COL10 = 0">
                  <xsl:value-of select="COL11 div COL10"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
			 </xsl:variable>

            <xsl:variable name="AvgPX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11 div COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$AvgPX &gt; 0">
                  <xsl:value-of select="$AvgPX"/>
                </xsl:when>
                <xsl:when test="$AvgPX &lt; 0">
                  <xsl:value-of select="$AvgPX * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varPosition &gt; 0">
                  <xsl:value-of select="$varPosition"/>
                </xsl:when>
                <xsl:when test="$varPosition &lt; 0">
                  <xsl:value-of select="$varPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
			  
            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>
			  
            <xsl:variable name="Currency">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

			  <xsl:variable name="varLeadingCurrency">
				  <xsl:choose>
					  <xsl:when test="(COL7='AUSTRALIAN DOLLAR SPOT' or COL7='NEW ZEALAND DOLLAR SPOT' or COL7='EURO SPOT' or COL7='GR BRITISH POUND SPOT')">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'0'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
  

			  <SideTagValue>
				  <xsl:choose>
					  <xsl:when test="$varLeadingCurrency='1'">
						  <xsl:choose>
							  <xsl:when test="contains($varSide,'Purchase an Asset')">
								  <xsl:value-of select="'1'"/>
							  </xsl:when>
							  <xsl:when test="contains($varSide,'Sell an Asset')">
								  <xsl:value-of select="'2'"/>
							  </xsl:when>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:when test="$varLeadingCurrency !='1'">
						  <xsl:choose>
							  <xsl:when test="contains($varSide,'Purchase an Asset')">
								  <xsl:value-of select="'2'"/>
							  </xsl:when>
							  <xsl:when test="contains($varSide,'Sell an Asset')">
								  <xsl:value-of select="'1'"/>
							  </xsl:when>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </SideTagValue>
   
            <CounterPartyID>             
                  <xsl:value-of select="'135'"/>              
            </CounterPartyID>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL3"/>
            </xsl:variable>
			  
            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL3"/>
            </xsl:variable>
			  
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="$varPositionSettlementDate"/>
            </PositionSettlementDate>


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


