<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
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

        <xsl:variable name="varFXRate">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($varFXRate)">
          <PositionMaster>

            <xsl:variable name="varBaseCurrency">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="varSettlCurrency">
              <xsl:value-of select="substring-before(COL1,' ')"/>
            </xsl:variable>
            <SettlementCurrency>
              <xsl:value-of select="$varSettlCurrency"/>
            </SettlementCurrency>

            <xsl:variable name="FXRate">
              <xsl:choose>
                <xsl:when test ="$varSettlCurrency='GBP' or $varSettlCurrency='EUR' or $varSettlCurrency='AUD' or $varSettlCurrency='NZD'">
                  <xsl:value-of select="1 div $varFXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varFXRate"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <ForexPrice>
              <xsl:choose>
                <xsl:when test="$FXRate &gt; 0">
                  <xsl:value-of select="$FXRate"/>

                </xsl:when>
                <xsl:when test="$FXRate &lt; 0">
                  <xsl:value-of select="$FXRate * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>

              </xsl:choose>
            </ForexPrice>


            <xsl:variable name="varDay">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL2),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL2,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL2,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL2),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL2,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL2,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varMonth">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL2),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL2,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL2,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL2),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL2,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL2,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL2,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL2,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			
            <Date>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </Date>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>