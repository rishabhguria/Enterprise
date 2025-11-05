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

  <xsl:template match="/">

    <DocumentElement>



      <xsl:for-each select ="//PositionMaster">

		  <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($MarkPrice) ">



          <PositionMaster>

			  <xsl:variable name="PB_NAME">
				  <xsl:value-of select="'GSEC'"/>
			  </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL29"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>


			  <xsl:variable name="PB_ROOT_NAME">
				  <xsl:value-of select="substring(COL33,1,2)"/>
			  </xsl:variable>

			  <xsl:variable name="PB_YELLOW_NAME">
				  <xsl:value-of select="substring-after(COL33,' ')"/>
			  </xsl:variable>
			  <xsl:variable name ="PRANA_ROOT_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
			  </xsl:variable>
			  <xsl:variable name ="PRANA_ROOT_NAME1">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@ExchangeCode"/>
			  </xsl:variable>
			  <xsl:variable name ="FUTURE_EXCHANGE_CODE">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
			  </xsl:variable>

			  <xsl:variable  name="FUTURE_FLAG">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
			  </xsl:variable>

			  <xsl:variable name="MonthCode">
				  <xsl:value-of select="substring(normalize-space(COL33),3,1)"/>
			  </xsl:variable>

			  <xsl:variable name="ExchangeCode">
				  <xsl:choose>
					  <xsl:when test="$PRANA_ROOT_NAME1!=''">
						  <xsl:value-of select="$PRANA_ROOT_NAME1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:variable name="Year" select="substring(normalize-space(COL33),4,1)"/>

			  <xsl:variable name="MonthYearCode">
				  <xsl:choose>
					  <xsl:when test="$FUTURE_FLAG!=''">
						  <xsl:value-of select="concat($Year,$MonthCode)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="concat($MonthCode,$Year)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="Underlying">
				  <xsl:choose>
					  <xsl:when test="$PRANA_ROOT_NAME!=''">
						  <xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <xsl:variable name="Future" select="concat($Underlying,' ',$MonthYearCode,$ExchangeCode)"/>

			  <xsl:variable name="Symbol" select="substring-before(COL33,' ')"/>

			  <Symbol>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>


					  <xsl:when test="COL37='OPTION'">
						  <xsl:value-of select="''"/>

					  </xsl:when>

					  <xsl:when test="COL37='FUTURE'">
						  <xsl:value-of select="$Future"/>
					  </xsl:when>
					  <xsl:when test="COL37='FUTFOP'">
						  <xsl:value-of select="concat(substring(COL33,1,2),' ',substring(COL33,3,3),substring-before(substring-after(COL33,' '),' '))"/>
					  </xsl:when>

					  <xsl:when test="COL37='EQUITY'">
						  <xsl:value-of select="substring-before(COL33,' ')"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>

				  </xsl:choose>

			  </Symbol>
			  
				  <xsl:variable name="Underlyer" >

					  <xsl:choose>
						  <xsl:when test="COL37='FUTFOP' or COL37='FUTURE'">
							  <xsl:value-of select="substring(COL33,1,2)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

		

			  <xsl:variable name="Prana_Multiplier">
				  <xsl:value-of select ="document('../ReconMappingXML/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$Underlyer]/@Multiplier"/>
			  </xsl:variable>



			  <xsl:variable name="MarkPrice1">
				  <xsl:choose>
					  <xsl:when test="number($Prana_Multiplier)">
						  <xsl:value-of select="$MarkPrice div $Prana_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$MarkPrice"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice1 &gt; 0">
                  <xsl:value-of select="$MarkPrice1"/>

                </xsl:when>
                <xsl:when test="$MarkPrice1 &lt; 0">
                  <xsl:value-of select="$MarkPrice1 * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="COL37='OPTION'">
                  <xsl:value-of select="concat(COL43,'U')"/>

                </xsl:when>

                <xsl:when test="COL37='FUTURE'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="COL37='EQUITY'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name="Date" select="COL2"/>

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