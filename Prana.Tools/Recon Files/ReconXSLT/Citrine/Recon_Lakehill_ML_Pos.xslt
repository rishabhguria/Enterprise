<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>

		  <!--Fund -->
		  <xsl:variable name = "PB_FUND_NAME" >
			  <xsl:value-of select="COL4"/>
		  </xsl:variable>

		  <xsl:variable name="PRANA_FUND_NAME">
			  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Monarch']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
		  </xsl:variable>

		  <xsl:if test ="($varInstrument='0' or $varInstrument='1' or $varInstrument='B' or $varInstrument='J') and $PRANA_FUND_NAME!= '' and COL14 !=''">

          <PositionMaster>

            
            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select="''"/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>
			  
			  <xsl:variable name="varPBSymbol" select="substring(COL20,1,21)"/>
			  <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL10)"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>
			  
			  <xsl:choose>
				  			  
				  <xsl:when test ="$varInstrument='B' or $varInstrument='J'">
					  <Symbol>
						  <xsl:value-of select="''"/>
					  </Symbol>
					  <IDCOOptionSymbol>
						  <xsl:value-of select="concat($varPBSymbol,'U')"/>
					  </IDCOOptionSymbol>
				  </xsl:when>
				 		  
				   <xsl:otherwise>
					  <Symbol>
						  <xsl:value-of select="normalize-space($varPBSymbol)"/>
					  </Symbol>
					  <IDCOOptionSymbol>
						  <xsl:value-of select="''"/>
					  </IDCOOptionSymbol>
				  </xsl:otherwise>
			  </xsl:choose>

			 
            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when test ="$varInstrument='0' or $varInstrument='1'">
                <PBAssetName>
                  <xsl:value-of select="'Equity'"/>
                </PBAssetName>
              </xsl:when>
              <xsl:when test ="$varInstrument='B' or $varInstrument='J' ">
                <PBAssetName>
                  <xsl:value-of select="'EquityOption'"/>
                </PBAssetName>
              </xsl:when>
              <xsl:otherwise>
                <PBAssetName>
                  <xsl:value-of select="''"/>
                </PBAssetName>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="COL14 &gt; 0 and $varInstrument='0' or $varInstrument='1'">
                <Quantity>
                  <xsl:value-of select="COL14"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL14 &lt; 0 and $varInstrument='0'">
                <Quantity>
                  <xsl:value-of select="COL14"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
				<xsl:when test="COL14 &gt; 0 and ($varInstrument='B' or $varInstrument='J')">
					<Quantity>
						<xsl:value-of select="COL14"/>
					</Quantity>
					<Side>
						<xsl:value-of select="'Buy'"/>
					</Side>
				</xsl:when>
				<xsl:when test="COL14 &lt; 0 and ($varInstrument='B' or $varInstrument='J')">
					<Quantity>
						<xsl:value-of select="COL14"/>
					</Quantity>
					<Side>
						<xsl:value-of select="'Sell'"/>
					</Side>
				</xsl:when>
              <xsl:otherwise>
                <Side>
                  <xsl:value-of select="''"/>
                </Side>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL13))">
                <MarkPrice>
                  <xsl:value-of select="COL13"/>
                </MarkPrice>
              </xsl:when>
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose>
			  
			  <SMRequest>
				  <xsl:value-of select ="'TRUE'"/>
			  </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
