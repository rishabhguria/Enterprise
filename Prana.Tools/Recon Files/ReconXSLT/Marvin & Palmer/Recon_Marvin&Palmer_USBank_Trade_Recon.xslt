<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
			  <xsl:with-param name="Number" select="normalize-space(COL20)" />
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="''" />
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
			  </xsl:variable>

			  <xsl:variable name="varSymbol">
				  <xsl:value-of select="normalize-space(COL7)"/>
			  </xsl:variable>

			    <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
				  <xsl:when test="$varSymbol!=''">
					  <xsl:value-of select="$varSymbol"/>
				  </xsl:when>				  
                <xsl:otherwise>
                 <xsl:value-of select="''" />
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
           
			  <xsl:variable name="PB_FUND_NAME" select="COL4" />
			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
			  </xsl:variable>
			  <AccountName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select="$PRANA_FUND_NAME" />
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PB_FUND_NAME" />
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>


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

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
				  <xsl:with-param name="Number" select="COL21"/>
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

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL24"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

			  <xsl:variable name="varfee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="normalize-space(COL26)"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <SecFee>
				  <xsl:choose>
					  <xsl:when test="$varfee &gt; 0">
						  <xsl:value-of select="$varfee"/>
					  </xsl:when>
					  <xsl:when test="$varfee &lt; 0">
						  <xsl:value-of select="$varfee * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </SecFee>

			
            <xsl:variable name="Currency" select="''"/>
            <CurrencySymbol>
              <xsl:value-of select="$Currency"/>
            </CurrencySymbol>

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
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

            <TradeDate>
              <xsl:value-of select="COL2"/>
            </TradeDate>

            <SettlementDate>
              <xsl:value-of select ="COL5"/>
            </SettlementDate>

           <xsl:variable name ="Side" >
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            <Side>
				<xsl:choose>
					<xsl:when test="$Quantity &gt; 0">
						<xsl:value-of select="'Buy'"/>
					</xsl:when>
					<xsl:when test="$Quantity &lt; 0">
						<xsl:value-of select="'Sell'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			  
			  <PBAssetType>
				  <xsl:value-of select="COL45"/>
			  </PBAssetType>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


