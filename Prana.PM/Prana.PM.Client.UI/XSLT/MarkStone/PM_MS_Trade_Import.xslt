<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
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
            <xsl:with-param name="Number" select="COL1"/>
          </xsl:call-template>
        </xsl:variable>

        <!--<xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(COL7 * 3)"/>
          </xsl:call-template>
        </xsl:variable>-->

        <xsl:if test="number($Position) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="Assettype">
              <xsl:choose>
                <xsl:when test="contains(COL9,'CALL') or contains(COL9,'PUT')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->




            <xsl:variable name="Symbol" >
              <xsl:choose>
                <xsl:when test="contains(COL2,'_')">
					
                  <xsl:value-of select="translate(normalize-space(substring-before(COL2,'_')),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="translate(normalize-space(COL2),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

               
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>


            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$Assettype='Option'">
                  <xsl:value-of select="concat($Symbol,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/AccountData[@PBAccountCode=$PB_FUND_NAME]/@PranaAccount"/>
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

            <xsl:variable name="Side" select="COL11"/>


            <SideTagValue>

            
                  <!--<xsl:choose>
                    <xsl:when test="$Position &gt; 0">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$Position &lt; 0">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>

                  </xsl:choose>-->
				<xsl:choose>
					<xsl:when test="$Side = 'by' or $Side = 'li'">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="$Side = 'sl'">
						<xsl:value-of select="'2'"/>
					</xsl:when>
				</xsl:choose>
                

            </SideTagValue>


			  <xsl:variable name="COL6">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL6"/>
				  </xsl:call-template>
			  </xsl:variable>




			  <xsl:variable name="Costbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6 div $Position"/>
              </xsl:call-template>
            </xsl:variable>


            <CostBasis>
              <xsl:choose>

                <xsl:when test="$Costbasis &gt; 0">
                  <xsl:value-of select="$Costbasis"/>
                </xsl:when>

                <xsl:when test="$Costbasis &lt; 0">
                  <xsl:value-of select="$Costbasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>


            </CostBasis>

          


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name ="Date" select="COL12"/>


            <!--<xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>-->



            <PositionStartDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="$Date"/>


            </PositionStartDate>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>