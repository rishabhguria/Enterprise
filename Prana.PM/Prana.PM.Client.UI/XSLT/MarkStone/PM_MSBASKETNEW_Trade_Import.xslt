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
            <xsl:with-param name="Number" select="COL3"/>
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




            <!--<xsl:variable name="Symbol" >
              --><!--<xsl:choose>
                <xsl:when test="contains(COL2)">--><!--
					
                  --><!--<xsl:value-of select="translate(normalize-space(substring-before(COL2,'_')),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="translate(normalize-space(COL2),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
                </xsl:otherwise>
              </xsl:choose>--><!--
            </xsl:variable>-->

            <Symbol>  
                  <xsl:value-of select="COL4"/>             
            </Symbol>

            <AccountName>     
                  <xsl:value-of select ="''"/>               
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

            <xsl:variable name="Side" select="COL2"/>


            <SideTagValue>
				<xsl:choose>
					<xsl:when test="$Side = 'B'">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="$Side = 'S'">
						<xsl:value-of select="'2'"/>
					</xsl:when>
				</xsl:choose>
                

            </SideTagValue>

		

            <CostBasis>
				<xsl:value-of select="COL5"/>
            </CostBasis>
			  
			 



			  <PositionStartDate>            
              <xsl:value-of select="COL1"/>
            </PositionStartDate>
          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>