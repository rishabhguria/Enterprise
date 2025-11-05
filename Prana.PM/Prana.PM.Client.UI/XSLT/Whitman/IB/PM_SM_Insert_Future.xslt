<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

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


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

	<xsl:template name="MonthCodeVar">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month='F' ">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='G' ">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='H' ">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='J' ">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='K' ">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='M' ">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='N'  ">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='Q'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='U' ">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='V' ">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month='X' ">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month='Z' ">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	

  <!--<xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL18='Option'">

      -->
  <!--</xsl:otherwise>-->
  <!--
      -->
  <!--

			</xsl:choose>-->
  <!--
    </xsl:if>-->
  <!--
  </xsl:template>-->

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL14"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) ">

          <PositionMaster>


            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Demo'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL15)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol" select="normalize-space(COL1)"/>




            <TickerSymbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="COL1!=''">
                  <xsl:value-of select="COL1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </TickerSymbol>

            <AssetID>
              <xsl:value-of select="3"/>
            </AssetID>

			  <CurrencyID>
				  <xsl:value-of select="1"/>
			  </CurrencyID>

            <PutOrCall>
              <xsl:choose>

                <xsl:when test="contains(substring(substring-after(COL1,' '),5,1),'P')">
                  <xsl:value-of select="'0'"/>
                </xsl:when>





                <xsl:when test="contains(substring(substring-after(COL1,' '),5,1),'C')">
                  <xsl:value-of select="'1'"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="'-1'"/>
                </xsl:otherwise>

              </xsl:choose>
            </PutOrCall>


            <Multiplier>
              <xsl:value-of select="COL14"/>
            </Multiplier>


                   <!--<xsl:variable name="Month" select="substring-before(COL17,'/')"/>-->
            <xsl:variable name="Date" select="substring-before(substring-after(COL17,'/'),'/')"/>
            <xsl:variable name="Year" select="concat('201',substring(substring-before(substring-after(COL1,' '),'-'),2,1))"/>


			  <xsl:variable name="Month" select="substring(substring-before(substring-after(COL1,' '),'-'),1,1)"/>

			  <xsl:variable name ="varMonthCode">
				  <xsl:call-template name ="MonthCodeVar">
					  <xsl:with-param name ="Month" select ="$Month"/>

				  </xsl:call-template>
			  </xsl:variable>


            <!--<ExpirationDate>
              <xsl:value-of select="concat($varMonthCode,'/','18','/',$Year)"/>
              <xsl:value-of select="COL17"/>
            </ExpirationDate>-->

			  <ExpirationDate>
				  <xsl:choose>
					  <xsl:when test="contains(COL17,'#N/A Invalid Security')">
						  <xsl:value-of select="concat($varMonthCode,'/','18','/',$Year)"/>
					  </xsl:when>
					  <!--<xsl:when test="COL17!=''">
						  <xsl:value-of select="COL17"/>
					  </xsl:when>-->
					 
					  <xsl:otherwise>
						  <xsl:value-of select="COL17"/>
						  <!--<xsl:value-of select="concat($Month,'/',$Date,'/',$Year)"/>-->
					  </xsl:otherwise>
				  </xsl:choose>
				 
				 
				 
			  </ExpirationDate>

            <UnderLyingSymbol>
              <xsl:value-of select="COL11"/>
            </UnderLyingSymbol>

            <UnderLyingID>
              <xsl:value-of select="2"/>
            </UnderLyingID>

            <xsl:variable name="StrikePrice" select="COL15"/>


            <!--<StrikePrice>
              <xsl:choose>
                <xsl:when test="number($StrikePrice)">
                  <xsl:value-of select="$StrikePrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StrikePrice>-->

            <AUECID>
              <xsl:value-of select="97"/>
            </AUECID>

			<ExchangeID>
				  <xsl:value-of select="88"/>
			 </ExchangeID>

            <BloombergSymbol>
              <xsl:value-of select="COL5"/>
            </BloombergSymbol>

            <ProxySymbol>
              <xsl:value-of select="COL13"/>
            </ProxySymbol>




            <LongName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </LongName>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
