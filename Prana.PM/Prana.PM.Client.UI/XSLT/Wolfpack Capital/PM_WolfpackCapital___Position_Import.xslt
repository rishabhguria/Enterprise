<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'%',''),$SingleQuote,''))"/>
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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL16)"/>
          </xsl:call-template>
        </xsl:variable>


		  <xsl:if test ="$Position and (COL2='Wolfpack Capital Partners' or contains(COL1,'Funds'))">


          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

		
			<xsl:variable name="varSedol">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>
			
		
            
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
             
                <xsl:when test="$varSedol!='*' or $varSedol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			
		
			<SEDOL>
              <xsl:choose>
				  <xsl:when test="$varSedol!='*' or $varSedol!=''">
					  <xsl:value-of select="$varSedol"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>
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

           

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
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

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17 div Position"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
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

		


			  <SideTagValue>
				  <xsl:choose>
					  <xsl:when test="$Position &gt; 0">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  <xsl:when test="$Position &lt; 0">
						  <xsl:value-of select="'5'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </SideTagValue>


			  <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
			  <xsl:variable name="inputDate" select="normalize-space(COL22)"/>

			  <xsl:variable name="Year" select="substring($inputDate, 1, 4)"/>
			  <xsl:variable name="Month" select="substring($inputDate, 5, 2)"/>
			  <xsl:variable name="Day" select="substring($inputDate, 7, 2)"/>

			  <xsl:variable name="FormattedDate">
				  <xsl:value-of select="concat($Month, '/', $Day, '/', $Year)"/>
			  </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="COL1"/>
            </PositionStartDate>


            <PositionSettlementDate>
              <xsl:value-of select="''"/>
            </PositionSettlementDate>

			  <xsl:variable name="PB_CURRENCY_NAME">
				  <xsl:value-of select="COL13"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_CURRENCY_ID">
				  <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
			  </xsl:variable>

			  <CurrencyID>
				  <xsl:choose>
					  <xsl:when test="number($PRANA_CURRENCY_ID)">
						  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CurrencyID>
			  
			  <FXRate>
				  <xsl:choose>
					  <xsl:when test="$PB_CURRENCY_NAME ='USD'">
						  <xsl:value-of select="'1'"/>
					  </xsl:when>
					  
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </FXRate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>
  
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>