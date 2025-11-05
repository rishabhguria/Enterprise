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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
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

  <xsl:template match="/">

    <DocumentElement>



      <xsl:for-each select ="//PositionMaster">



        <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL30"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($MarkPrice) and contains(COL51,'CASH') !='true'">



          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Cloverdale'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="PB_SUFFIX_NAME">
              <xsl:value-of select="COL44"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
            </xsl:variable>

			  <xsl:variable name="Symbol">
				  <xsl:choose>
					  <xsl:when test="contains(COL8,'.')">
						  <xsl:value-of select="substring-before(COL8,'.')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="(COL8)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="contains(COL50,'CALL') or contains(COL50,'PUT')">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL50,'FX FORWARDS')">
						  <xsl:value-of select="'FX'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <Symbol>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="$Asset='FX'">
						  <xsl:value-of select="concat(translate(substring-before(COL6,' '),'/','-'),' ',translate(substring-after(COL6,' '),'/',''))"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Equity'">
						  <xsl:value-of select="concat(translate($Symbol,$lower_CONST,$upper_CONST),$PRANA_SUFFIX_NAME)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>

				  </xsl:choose>

            </Symbol>

			  <xsl:variable name="Underlying" select="substring-before(COL8,'1')"/>
			  <xsl:variable name="undspaces">
				  <xsl:if test="$Asset='Option'">
				  <xsl:call-template name="spaces">
					  <xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
				  </xsl:call-template>
				  </xsl:if>
			  </xsl:variable>
			  <xsl:variable name="IdcoLast" select="substring(COL8,string-length($Underlying)+1)"/>
			  <xsl:variable name="Idco">
				  <xsl:value-of select="concat($Underlying,$undspaces,$IdcoLast,'U')"/>
			  </xsl:variable>


			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="$Idco"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>


            <!--<xsl:variable name="Side" select="COL1"/>


            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Side='Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:when test="$Side='Long'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>-->


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="Date" select="''"/>

            <Date>
              <xsl:value-of select="$Date"/>
            </Date>



          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>