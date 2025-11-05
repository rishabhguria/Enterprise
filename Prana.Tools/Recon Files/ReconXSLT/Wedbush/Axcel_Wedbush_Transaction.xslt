<?xml version="1.0" encoding="utf-8" ?>
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
      <xsl:for-each select="//Comparision">

	  <xsl:variable name="Quantity">
			<xsl:call-template name="Translate">
			<xsl:with-param name="Number" select="COL4"/>
			</xsl:call-template>
	  </xsl:variable>
				
		  <xsl:if test="number($Quantity)">
          <PositionMaster>

		    <xsl:variable name = "PB_NAME" select ="'WedbushP'"/>
			
			  <xsl:variable name = "PB_FUND_NAME" >
				  <xsl:value-of select="COL2"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <AccountName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select="$PRANA_FUND_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select='$PB_FUND_NAME'/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>
			
			<xsl:variable name = "PB_SYMBOL_NAME" select ="COL18"/>
			 
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>
			 
			 <xsl:variable name="varSingleQuote">'</xsl:variable>
			   
			   <xsl:variable name="varSymbol">
					  <xsl:choose>
								<xsl:when test="contains(COL5,$varSingleQuote)">
									<xsl:value-of select="translate(COL5,$varSingleQuote,'-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
				  </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
				
				<xsl:when test ="$varSymbol != ''">
                  <xsl:value-of select ="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                 <xsl:value-of select = "$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

			  <Side>
				  <xsl:choose>
					  <xsl:when test="COL3 != '' and COL3 = 'P'">
						  <xsl:value-of select="'Buy'"/>
					  </xsl:when>

					  <xsl:when test="COL3 != '' and COL3 = 'S'">
						  <xsl:value-of select="'Sell'"/>
					  </xsl:when>

					  <xsl:when test="COL3 != '' and COL3 = 'SS'">
						<xsl:value-of select="'Sell short'"/>
					</xsl:when>

					<xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <Quantity>
				<xsl:choose>
					<xsl:when test ="$Quantity &gt; 0">
						<xsl:value-of select="COL4"/>
					</xsl:when>
					<xsl:when test ="$Quantity &lt; 0">
						<xsl:value-of select="COL4 * (-1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
            </Quantity>

			<xsl:variable name= "AvgPrice" select="COL6"/>
			
            <AvgPX>
              <xsl:choose>
                <xsl:when test="number($AvgPrice)">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

			
	  <xsl:variable name="Notional">
			<xsl:call-template name="Translate">
			<xsl:with-param name="Number" select="COL17"/>
			</xsl:call-template>
	  </xsl:variable>
	  
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test ="$Notional &lt; 0">
                  <xsl:value-of select="COL17*(-1)"/>
                </xsl:when>
                <xsl:when test ="$Notional &gt; 0">
                  <xsl:value-of select="COL17"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
